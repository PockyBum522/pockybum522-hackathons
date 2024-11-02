using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace TnidLoginProxy.TnidApi;

public static class TnidPeopleSearcher
{
	private static GraphQLHttpClient? _graphQlClient;

	static TnidPeopleSearcher()
	{
		_graphQlClient ??= new GraphQLHttpClient("https://api.staging.v2.tnid.com/company", new NewtonsoftJsonSerializer());
		
		_graphQlClient.HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SECRETS.COMPANY_DARTMOUTH_AVE_CREATIONS_BEARER_TOKEN);
	}
	
    public static async Task<dynamic> SearchByPhoneNumber(string phoneNumber)
    {

        var searchPersonRequest = new GraphQLRequest
        {
            Query = @"
			        query (
						$name: String
						$email: String
						$telephoneNumber: String
						$limit: Int
					  ) {
						users (
  						name: $name
  						email: $email
  						telephoneNumber: $telephoneNumber
  						limit: $limit
						) {
  						id
  						firstName
  						lastName
  						middleName
  						username
						}
					  }
			        ",
            Variables = new
            {
                telephoneNumber = phoneNumber
            }
        };

        if (_graphQlClient is null)
        {
	        throw new NullReferenceException("_graphQlClient is null");
        }
        
        return await _graphQlClient.SendQueryAsync<dynamic>(searchPersonRequest);
    }
}