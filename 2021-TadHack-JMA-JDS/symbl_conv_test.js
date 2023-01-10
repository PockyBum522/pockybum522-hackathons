const request = require('request');
const accessToken = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IlFVUTRNemhDUVVWQk1rTkJNemszUTBNMlFVVTRRekkyUmpWQ056VTJRelUxUTBVeE5EZzFNUSJ9.eyJodHRwczovL3BsYXRmb3JtLnN5bWJsLmFpL3VzZXJJZCI6IjQ2ODk5NzY4NDAxNTkyMzIiLCJpc3MiOiJodHRwczovL2RpcmVjdC1wbGF0Zm9ybS5hdXRoMC5jb20vIiwic3ViIjoiWVlrakVPVEZZQzZUQmVhY2VJQ3hoMm1oZ0c4SFJ5ZmlAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vcGxhdGZvcm0ucmFtbWVyLmFpIiwiaWF0IjoxNjMyNjEwOTI5LCJleHAiOjE2MzI2OTczMjksImF6cCI6IllZa2pFT1RGWUM2VEJlYWNlSUN4aDJtaGdHOEhSeWZpIiwiZ3R5IjoiY2xpZW50LWNyZWRlbnRpYWxzIn0.FfT6BBELmimxQcV3Dh2ajYAV3tZTcW8y3ggV6DuCDe10taHABr0apNO4hkeHrw2PFcUeApdd1VjQbafo2HaxQod_1r5d_7UOJvDUFqhmkyaGUWr3pf1zE-Sx0b7d8sh32gMLfz3jnyapIx8WEd2kTowKD23Lm3iKgk28JjygNSUCLFzysrZVYERmPJGeRUSROXW7lYlmNaAgRQ1uQurawTmD1TwhMSctKl8nvLluS_nWwdvX0Op-hPudY1xRW7gNpn3-5_DeFbgut1NpdqQq3nxvkzbMS_9GVon5ojap3ym_CA5Jf3FwADpwPMLoG4vuAECi3MybMs51wteF5rZNBQ';
const conversationId = '5511839635996672';

request.get({
    url: `https://api.symbl.ai/v1/conversations/${conversationId}/messages?sentiment=true`,
    headers: { 'Authorization': `Bearer ${accessToken}` },
    json: true
}, (err, response, body) => {
    for(let mes in body.messages) {
        console.log(body.messages[mes].text);
        console.log(body.messages[mes].sentiment.suggested);
        console.log("_-_-_-_-_-_-");
    }
});