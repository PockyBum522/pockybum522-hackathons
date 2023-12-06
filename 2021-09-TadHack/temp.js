const request = require('request');
const authToken = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IlFVUTRNemhDUVVWQk1rTkJNemszUTBNMlFVVTRRekkyUmpWQ056VTJRelUxUTBVeE5EZzFNUSJ9.eyJodHRwczovL3BsYXRmb3JtLnN5bWJsLmFpL3VzZXJJZCI6IjQ2ODk5NzY4NDAxNTkyMzIiLCJpc3MiOiJodHRwczovL2RpcmVjdC1wbGF0Zm9ybS5hdXRoMC5jb20vIiwic3ViIjoiWVlrakVPVEZZQzZUQmVhY2VJQ3hoMm1oZ0c4SFJ5ZmlAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vcGxhdGZvcm0ucmFtbWVyLmFpIiwiaWF0IjoxNjMyNjEwOTI5LCJleHAiOjE2MzI2OTczMjksImF6cCI6IllZa2pFT1RGWUM2VEJlYWNlSUN4aDJtaGdHOEhSeWZpIiwiZ3R5IjoiY2xpZW50LWNyZWRlbnRpYWxzIn0.FfT6BBELmimxQcV3Dh2ajYAV3tZTcW8y3ggV6DuCDe10taHABr0apNO4hkeHrw2PFcUeApdd1VjQbafo2HaxQod_1r5d_7UOJvDUFqhmkyaGUWr3pf1zE-Sx0b7d8sh32gMLfz3jnyapIx8WEd2kTowKD23Lm3iKgk28JjygNSUCLFzysrZVYERmPJGeRUSROXW7lYlmNaAgRQ1uQurawTmD1TwhMSctKl8nvLluS_nWwdvX0Op-hPudY1xRW7gNpn3-5_DeFbgut1NpdqQq3nxvkzbMS_9GVon5ojap3ym_CA5Jf3FwADpwPMLoG4vuAECi3MybMs51wteF5rZNBQ';
const jobId = '1cca9a02-28f5-430e-afcc-0111ee1f7d4a';

request.get({
    url: `https://api.symbl.ai/v1/job/${jobId}`,
    headers: { 'Authorization': `Bearer ${authToken}` },
    json: true
}, (err, response, body) => {
  console.log(body);
});