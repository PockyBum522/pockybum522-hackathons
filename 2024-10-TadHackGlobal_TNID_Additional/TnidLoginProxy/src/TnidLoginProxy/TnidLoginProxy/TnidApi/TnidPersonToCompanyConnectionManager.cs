using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace TnidLoginProxy.TnidApi;

public static class TnidPersonToCompanyConnectionManager
{
	private static GraphQLHttpClient? _graphQlClient;

	static TnidPersonToCompanyConnectionManager()
	{
		_graphQlClient ??= new GraphQLHttpClient("https://api.staging.v2.tnid.com/company", new NewtonsoftJsonSerializer());
		
		_graphQlClient.HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SECRETS.COMPANY_DARTMOUTH_AVE_CREATIONS_BEARER_TOKEN);
	}
	
	// ReSharper disable once InconsistentNaming
	public static async Task<dynamic> SendConnectionRequest(string userId, string b2cConnectionType)
    {
        var connectionRequest = new GraphQLRequest
        {
            Query = @"
					mutation (
						$invitedUserId: ID!
						$connectionType: B2cConnectionType!
					  ) {
						createB2cConnectionRequest (
  						invitedUserId: $invitedUserId
  						connectionType: $connectionType
						) {
  						id
  						status
  						type
  						insertedAt
  						respondedAt
  						updatedAt
  						company {
    						id
  						}
  						user {
    						id
  						}
  						invitedUser {
    						id
  						}
						}
					  }
			        ",
            Variables = new
            {
                invitedUserId = userId,
                connectionType = b2cConnectionType
            }
        };

        if (_graphQlClient is null)
        {
	        throw new NullReferenceException("_graphQlClient is null");
        }
        
        return await _graphQlClient.SendQueryAsync<dynamic>(connectionRequest);
    }
}