using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace TnidLoginProxy.TnidApi;

public static class TnidCompanyConnectionManager
{
	private static GraphQLHttpClient? _graphQlClient;

	static TnidCompanyConnectionManager()
	{
		_graphQlClient ??= new GraphQLHttpClient("https://api.staging.v2.tnid.com/company", new NewtonsoftJsonSerializer());
		
		_graphQlClient.HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SECRETS.COMPANY_DARTMOUTH_AVE_CREATIONS_BEARER_TOKEN);
	}
	
	// ReSharper disable once InconsistentNaming
	public static async Task<dynamic> ShowPendingConnectionRequests()
    {
        var connectionRequest = new GraphQLRequest
        {
            Query = @"
					query (
						$userId: ID
						$includedType: B2cConnectionType
						$excludedType: B2cConnectionType
						$limit: Int
					  ) {
						pendingC2bConnectionRequests (
  						userId: $userId
  						includedType: $includedType
  						excludedType: $excludedType
  						limit: $limit
						) {
  						id
  						status
  						type
  						insertedAt
  						respondedAt
  						updatedAt
  						user {
    						id
  						}
  						invitedCompany {
    						id
  						}
  						respondedByUser {
    						id
  						}
						}
					  }
			        "
        };

        if (_graphQlClient is null)
        {
	        throw new NullReferenceException("_graphQlClient is null");
        }
        
        return await _graphQlClient.SendQueryAsync<dynamic>(connectionRequest);
    }
	
	
	public static async Task<dynamic> ListConnectionRequests()
	{      
		var connectionRequest = new GraphQLRequest
		{
			Query = @"
				query (
					$invitedUserId: ID
					$includedType: B2cConnectionType
					$excludedType: B2cConnectionType
					$limit: Int
				  ) {
					pendingB2cConnectionRequests (
  					invitedUserId: $invitedUserId
  					includedType: $includedType
  					excludedType: $excludedType
  					limit: $limit
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
				limit = 100
			}
		};

		if (_graphQlClient is null)
		{
			throw new NullReferenceException("_graphQlClient is null");
		}
        
		return await _graphQlClient.SendQueryAsync<dynamic>(connectionRequest);
	}
}