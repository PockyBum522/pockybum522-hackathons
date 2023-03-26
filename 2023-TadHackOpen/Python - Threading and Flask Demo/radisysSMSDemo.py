from flask import Flask, abort, request
import sys
import requests
import json
from apikey import apikey
from account_id import account_id

app = Flask(__name__)

_json_string = '''
{
	"From": "+12762586340",
	"To": "+14076322207",
	"Eml": "<?xml version='1.0' encoding='UTF-8'?><Response><Say>This is Demo</Say></Response>"
}
'''


class JsonPoster:
  def post_json_async(self, raw_json: str):
    headers = {
#"Content-Type": "application/json",
	  "apikey": apikey
    }
    response = requests.post("https://apigateway.engagedigital.ai/api/v1/accounts/"+account_id+"/call", data=raw_json, headers=headers)
    return response

@app.route("/")
def hello_world():
  return "<p>Hello, World!</p>"


@app.route('/detect/<x>/<y>/<z>', methods=['GET'])
def accelerometer_detect_change(x, y, z):
  if request.method == 'GET':
    print('This is error output', file=sys.stderr)
    if x == 2:  
      print("x is 2")
      json_poster = JsonPoster()
      response = json_poster.post_json_async(_json_string)
      deserialized_json = json.loads(response.text)
      pretty_json = json.dumps(deserialized_json, indent=2)
      print(response.text)
    return str(x) + str(y) + str(z)
  else:
    abort(405)

def call(phone_number):
  return response.text

if __name__ == "__main__": 
  print("before app.run")
  app.run(host='0.0.0.0', debug=True)
