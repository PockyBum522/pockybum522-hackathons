require('dotenv').config();
const telnyxApiKey = process.env.TELNYX_API_KEY
const symblAccessToken = process.env.SYMBL_API_KEY

const express = require('express')
const axios = require('axios')
const telnyx = require('telnyx')(telnyxApiKey);
const bodyParser = require('body-parser')

//console.log(`api key: ${telnyxApiKey}`);

// CONFIGURATION
const expressApp = express()
const port = 80

const responses = {
    400: 'Bad Request! Please refer docs for correct input fields.',
    401: 'Unauthorized. Please generate a new access token.',
    404: 'The conversation and/or it\'s metadata you asked could not be found, please check the input provided',
    429: 'Maximum number of concurrent jobs reached. Please wait for some requests to complete.',
    500: 'Something went wrong! Please contact support@symbl.ai'
}  

const incomingTelnyxWebhookEndpoint = 'onIncoming/session136';

const reviewPromptText = "We hope you had a great time at the museum! Please respond with a detailed review of your visit.";
let formattedPhoneNumber = `+14074632925`;

let lastTenReceivedMessageIDsArray = [];

var bodyParserOptions = {
    inflate: true,
    limit: '100kb',
    type: 'application/json'
};

expressApp.use(bodyParser.json(bodyParserOptions));

// MAIN CALL
startUserReviewProcess();

// Validation logic
function isIncomingMessage(reqBody) {
    return reqBody.data.event_type === "message.received";
}

function messagePreviouslyReceived(reqBody) {

    if(lastTenReceivedMessageIDsArray.includes(reqBody.data.id)) {
        return true;
    }

    lastTenReceivedMessageIDsArray.push(reqBody.data.id);

    return false;
}

// Main function, the endpoint below is called by Telnyx on message response
function startUserReviewProcess(){

    console.log();
    console.log();
    console.log();

    telnyx.messages
        .create(
        {
            'from': '+16182120374', // Your Telnyx number
            'to': formattedPhoneNumber,
            'text': reviewPromptText
        })
        .then(() => {

            console.log(`Review message sent for: ${ formattedPhoneNumber }`)
            console.log(`Now listening for response on: /${incomingTelnyxWebhookEndpoint}`)

        })
        .catch(
            (err) => {
                console.error(err)
        });
};

function createSymblJobFromSmsBody(smsResponseBody){

    return new Promise(
        (resolve, reject) => {
            
            // Symbl workers
            const symblRequestHeaders = {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${symblAccessToken}`
            };

            const symblSmsSubmitRequestJson = {
                "messages": [
                    {
                        "payload": {
                            "content": smsResponseBody.data.payload.text,
                            "contentType": "text/plain"
                        }
                    }
                ]
            };
            
            axios.post('https://api.symbl.ai/v1/process/text', symblSmsSubmitRequestJson, {
                headers: symblRequestHeaders
            })
            .then((res) => {

                const symblConversationId = res.data.conversationId;
                const symblJobId = res.data.jobId;

                console.log(` ${symblConversationId}  ${symblJobId}`)

                return resolve([symblConversationId, symblJobId]);

            }).catch((err) => {
                
                console.error(err);
                reject(err);

            })
    })
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function waitForJobCompletion(symblJobId){
    
    const symblRequestHeaders = {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${symblAccessToken}`
    };

    let jobStatus = 'in_progress';

    while(jobStatus !== 'completed'){

        console.log("Looooooooooop, brother")

        const p = axios.get(`https://api.symbl.ai/v1/job/${symblJobId}`, { 
            headers: symblRequestHeaders 
        });
        
        p.then((res) =>
            
            jobStatus = res.data.status

        )
        .catch((err) => console.log(err));            
        
        await sleep(1000);
        
        console.log(jobStatus)
    }
}

function getSymblSentiment(symblConversationId) {
    
    return new Promise((resolve, reject) => {

        const symblRequestHeaders = {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${symblAccessToken}`
        };

        axios.get(`https://api.symbl.ai/v1/conversations/${symblConversationId}/messages?sentiment=true`, { headers: symblRequestHeaders})
        .then((res) => {
            
            console.log("got further, in then beyond sentiment get")

            resolve(res);

        }).catch((err) => {
            console.error(err);
            reject(err);
        })       
    })
}

function prettyPrintSentiment(sentimentResponse){
    
    console.log();
    console.log(' ==================== SENTENCE FRAGMENTS ==================== ')
    console.log();

    for(let mes in sentimentResponse.data.messages) {
        
        console.log(' ==================== NEW FRAGMENT ==================== ')
        console.log();

        console.log(`\t ${ sentimentResponse.data.messages[mes].text }`);
        console.log(`\t\t\t Sentiment: ${sentimentResponse.data.messages[mes].sentiment.suggested}`);
        
        console.log();
        
    }

    console.log();
    console.log(' ==================== END SENTENCE FRAGMENTS ==================== ')
    console.log();

}
    
// Webhook endpoint that takes in all Telnyx responses
expressApp.post(`/${incomingTelnyxWebhookEndpoint}`, (req, res) => {
    
    if(!isIncomingMessage(req.body) || messagePreviouslyReceived(req.body)) {
        
        return;
    }

    console.log(`Creating job from SMS: ${req.body.data.payload.text}`)

    let symblConversationId = null;

    // Otherwise:
    createSymblJobFromSmsBody(req.body)
    .then((resData) => {
            
        symblConversationId = resData.conversationId;
        let symblJobId = resData.jobId;

        console.log(`Job created from SMS body, conversation ID: ${ symblConversationId } and jobId: ${ symblJobId }`);           

        return waitForJobCompletion(symblJobId);
        
    }).then(() => {
        
        // Job has finished
        return getSymblSentiment(symblConversationId);

    })
    .then((res) => 

        prettyPrintSentiment(res)

    )
    .catch((err) => console.error(err));    

})

expressApp.listen(port, () => {
    console.log(`Example app listening at http://localhost:${port}`);
})