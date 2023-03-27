import sys
import requests
import json

from flask import Flask, abort, request, Response
from urllib3 import response
from resources.apikey import apikey
from resources.account_id import account_id

app = Flask(__name__)

if __name__ == "__main__":

    print("")
    print(" * Starting trashcan notification controller...")
    print("")

    app.run(host='0.0.0.0', debug=True)

notify_jared_json = '''
{
    "From": "+12762586340",
    "To": "+14076322207",
    "Eml": "<?xml version='1.0' encoding='UTF-8'?><Response><Say>Your inside trash can is full and is not closed completely. I repeat, your inside trash can is full and is not closed completely. I repeat,  your inside trash can is full and is not closed completely. I repeat,  your inside trash can is full and is not closed completely.</Say></Response>"
}
'''