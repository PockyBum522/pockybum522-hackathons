require('secrets')
const express = require('express')
const axios = require('axios')
const bodyParser = require('body-parser')
const request = require('request');

const responses = {
    400: 'Bad Request! Please refer docs for correct input fields.',
    401: 'Unauthorized. Please generate a new access token.',
    404: 'The conversation and/or it\'s metadata you asked could not be found, please check the input provided',
    429: 'Maximum number of concurrent jobs reached. Please wait for some requests to complete.',
    500: 'Something went wrong! Please contact support@symbl.ai'
  }  

const app = express()
const port = 80

const incomingWebhookUrlBase = 'http://pockybum522.com';
const incomingWebhookEndpoint = 'onIncoming/session136';

const reviewPromptText = "We hope you had a great time at the museum! Please respond with a detailed review of your visit.";
let formattedPhoneNumber = `+14074632925`;

let lastTenReceivedIDsArray = [];

var bodyParserOptions = {
    inflate: true,
    limit: '100kb',
    type: 'application/json'
};

app.use(bodyParser.json(bodyParserOptions));

telnyx.messages
      .create(
      {
          'from': '+16182120374', // Your Telnyx number
          'to': formattedPhoneNumber,
          'text': reviewPromptText
      })
      .then(() => {
          console.log(`Review message sent for: ${ formattedPhoneNumber }`)
          console.log(`Now listening for response on: /${incomingWebhookEndpoint}`)
      })
      .catch(
          (err) => {
              console.error(err)
          }
      )

function isIncomingMessage(reqBody) {
    return reqBody.data.event_type === "message.received";
}

function messagePreviouslyReceived(reqBody) {
    if(lastTenReceivedIDsArray.includes(reqBody.data.id)) {
        return true;
    }

    lastTenReceivedIDsArray.push(reqBody.data.id);
    return false;
}

app.post(`/${incomingWebhookEndpoint}`, (req, res) => {
    let conversationId = null;
    let jobId = null;

    // Validate message
    if(!isIncomingMessage(req.body) || messagePreviouslyReceived(req.body)) {
        return;
    }

    const headers = {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${accessToken}`
    };

    const data = {
        "messages": [
        {
            "payload": {
            "content": req.body.data.payload.text,
            "contentType": "text/plain"
            }
        }
        ]
    };

    // const sendUserReview = new Request('', options);
    axios.post('https://api.symbl.ai/v1/process/text', data, {
        headers: headers
    })
    .then((res) => {
        conversationId = res.data.conversationId;
        jobId = res.data.jobId;
        console.log(conversationId);
        console.log(res.data.jobId);
        axios.get(`https://api.symbl.ai/v1/job/${res.data.jobId}`, {
            headers: headers
        })
        .then((res) => {
            checkIfCompleted();
        }).catch((err) => {
            console.error(err);
        })
    }).catch((err) => {
        console.error(err);
    });

    async function checkIfCompleted() {
        console.log('starting into loop');

        let result = {data: {status: {}}};
            
            do {
                result = await axios.get(`https://api.symbl.ai/v1/job/${jobId}`, { headers: headers }).catch((err) => {console.error(err)});
                console.log(`Status: ${result.data.status}`);
                setTimeout(() => {}, 200);
            } while(result.data.status !== 'completed')

            console.log("got past while loop");
            getSentiments();


        return result.data.status;
    }

    async function getSentiments() {
        
            axios.get(`https://api.symbl.ai/v1/conversations/${conversationId}/messages?sentiment=true`, { headers: headers})
            .then((res) => {
        
                console.log("do something with the sentiment");
        
                for(let mes in res.data.messages) {
                    console.log(res.data.messages[mes].text);
                    console.log(res.data.messages[mes].sentiment.suggested);
                    console.log("_-_-_-_-_-_-");
                }

                // console.log(res)
        
            }).catch((err) => {
                console.error(err);
            });
        
    }


    // fetch('https://api.symbl.ai/v1/process/text').then((res) => {
    //     console.log(`statusCode: ${res.statusCode}`);
    //     console.log(`Body: ${res.body}`);
    // })
    
    // Sending off user text for evaluation
    // function sendUserResponseForEvaluation() { 
    //     request(options, function (err, response) {
    //         const statusCode = response.statusCode;
    //         if (err || Object.keys(responses).indexOf(statusCode.toString()) !== -1) {
    //             throw new Error(responses[statusCode]);
    //         }
    //         console.log('Status code: ', statusCode);
    //         console.log('Body', response.body);
    //     });    
    // }

    // const resPromise = new Promise(sendUserResponseForEvaluation);

      // Send response

})

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`);
})


// set your access token here. See https://docs.symbl.ai/docs/developer-tools/authentication





/*
{
  data: {
    event_type: 'message.received',
    id: '9d9dfe67-7060-4254-af7e-3fcc61a8d686',
    occurred_at: '2021-09-25T22:12:55.637+00:00',
    payload: {
      cc: [],
      completed_at: null,
      cost: null,
      direction: 'inbound',
      encoding: 'GSM-7',
      errors: [],
      from: [Object],
      id: '3d6bf57e-b2e4-42fb-9dd2-9e024dac6abe',
      media: [],
      messaging_profile_id: '40017c1d-e7c0-4b79-badd-f237ba70c4b6',
      organization_id: '8bc85271-e344-48c3-875b-c22ee9805053',
      parts: 1,
      received_at: '2021-09-25T22:12:55.522+00:00',
      record_type: 'message',
      sent_at: null,
      subject: '',
      tags: [],
      text: 'THE MESSAGE',
      to: [Array],
      type: 'SMS',
      valid_until: null,
      webhook_failover_url: null,
      webhook_url: 'http://pockybum522.com/onIncoming/session136'
    },
    record_type: 'event'
  },
  meta: {
    attempt: 1,
    delivered_to: 'http://pockybum522.com/onIncoming/session136'
  }
}

*/

/*
[6:49 PM, 9/25/2021] David Sikes: symbl app id: 59596b6a454f544659433654426561636549437868326d686747384852796669
[6:48 PM, 9/25/2021] David Sikes: symbl secret: 34664e486c7051536762506770696b6e645f42616b58466147557049394c64726b33396a736735547a3165334d4d4273584e7a7638524b6f745a665748637157
*/
/*
curl -k -X POST "https://api.symbl.ai/oauth2/token:generate" \
     -H "accept: application/json" \
     -H "Content-Type: application/json" \
     -d $'{
      "type" : "application",
      "appId": "'59596b6a454f544659433654426561636549437868326d686747384852796669'",
      "appSecret": "'34664e486c7051536762506770696b6e645f42616b58466147557049394c64726b33396a736735547a3165334d4d4273584e7a7638524b6f745a665748637157'"
    }'
*/