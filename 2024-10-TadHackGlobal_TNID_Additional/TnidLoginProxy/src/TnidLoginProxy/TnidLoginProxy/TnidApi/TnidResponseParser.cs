namespace TnidLoginProxy.TnidApi;

public static class TnidResponseParser
{
    public static string GetUserFullName(dynamic tnidPeopleSearchApiRequest)
    {
        var responseData = tnidPeopleSearchApiRequest.Data;

        var firstUser = responseData.users[0];

        var usersFirstName = (string)(firstUser.firstName);
        var usersLastName = (string)(firstUser.lastName);
        
        return usersFirstName + " " + usersLastName; 
    }
    
    public static string GetUserId(dynamic responseData)
    {
        var firstUser = responseData.users[0];

        return (string)(firstUser.id);
    }
}
