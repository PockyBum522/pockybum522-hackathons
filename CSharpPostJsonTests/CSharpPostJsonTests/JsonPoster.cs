﻿using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSharpPostJsonTests;

public class JsonPoster
{
    public async Task<HttpResponseMessage> PostJsonAsync(string rawJson, string endpointUrl)
    {
        var json = JObject.Parse(rawJson);
        
        var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        using var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add("apikey", "eyJ4NXQiOiJZamd5TW1GalkyRXpNVEZtWTJNMU9HRmtaalV3TnpnMVpEVmhZVGRtTnpkaU9HUmhNR1kzWmc9PSIsImtpZCI6ImFwaV9rZXlfY2VydGlmaWNhdGVfYWxpYXMiLCJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJyYWRpc3lzQGNhcmJvbi5zdXBlciIsImFwcGxpY2F0aW9uIjp7Im93bmVyIjoicmFkaXN5cyIsInRpZXJRdW90YVR5cGUiOm51bGwsInRpZXIiOiJVbmxpbWl0ZWQiLCJuYW1lIjoicnN5cy0xMDAwNi10YWRoYWNrMjMuY29tIiwiaWQiOjE3MSwidXVpZCI6IjFlNmRkZGQ2LTc1MTMtNDYzZC1iNzM2LWIxN2RhZDgwMDQwZiJ9LCJpc3MiOiJodHRwczpcL1wvYXBpbS5lbmdhZ2VkaWdpdGFsLmFpOjQ0M1wvb2F1dGgyXC90b2tlbiIsInRpZXJJbmZvIjp7IlVubGltaXRlZCI6eyJ0aWVyUXVvdGFUeXBlIjoicmVxdWVzdENvdW50Iiwic3RvcE9uUXVvdGFSZWFjaCI6dHJ1ZSwic3Bpa2VBcnJlc3RMaW1pdCI6MCwic3Bpa2VBcnJlc3RVbml0IjpudWxsfX0sImtleXR5cGUiOiJQUk9EVUNUSU9OIiwic3Vic2NyaWJlZEFQSXMiOlt7InN1YnNjcmliZXJUZW5hbnREb21haW4iOiJjYXJib24uc3VwZXIiLCJuYW1lIjoiQ2FsbEFQSVByb2R1Y3QiLCJjb250ZXh0IjoiXC9hcGlcL3YxIiwicHVibGlzaGVyIjoicmFkaXN5cyIsInZlcnNpb24iOiIxLjAuMCIsInN1YnNjcmlwdGlvblRpZXIiOiJVbmxpbWl0ZWQifV0sImlhdCI6MTY3OTQxMzk3NSwianRpIjoiZDIyMTljNzQtZGY0Mi00ZmU3LWE1NzMtN2M4YjViZjQ4ZWViIn0=.lWNO2_wSgP58dC1dnA22d-41oakm0v-zbSnqE_JtPhdQ0TWUji1C_McssA6kOF32OiODOBcRYp8bZfK-Dv7a8bZ7ljQbWKPBbn2XGq1-sltdKF2tat2yvqVO4Lg7Xqu9Wd8F2pHiHAboz2vnorAP7uYFHbzebTCgsB1inXb4XB12QWaUsr4H7VelWgg3iR_BRk--gLm3viT0zZ9nVyd3EcPtD1zA6HY59_wkaX6oh6C-JvPJP_Iw9_JWouTnAUoRO-0lt47hBPKCLVsOD5UH8ivoVmYvD4_-_O2xJ3pnaQi3yKjnVQNIj2-s0NFspgNwNE4mU1tIGKX05Mds1qlg_Q==");

        var response = await httpClient.PostAsync(endpointUrl, content);

        return response;
    }
}