import json
import requests

from resources.account_id import account_id
from resources.apikey import apikey


class JsonPoster:
    def __init__(self):
        pass

    @staticmethod
    def post_json_to_radisys_api(raw_json_string):

        headers = {
          "Content-Type": "application/json",
          "apikey": apikey
        }

        response = requests.post("https://apigateway.engagedigital.ai/api/v1/accounts/"+account_id+"/call", data=raw_json_string, headers=headers)

        deserialized_response = json.loads(response.text)

        pretty_json_response = json.dumps(deserialized_response, indent=4)

        return response