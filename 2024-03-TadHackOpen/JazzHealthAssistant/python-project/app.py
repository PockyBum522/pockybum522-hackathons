from signalwire.voice_response import *
from flask import Flask, request, render_template
import sqlite3
import os
import logging
from pyngrok import ngrok

# if not os.environ['NGROK_URL']:
#     public_url = ngrok.connect(80)
#     os.environ['NGROK_URL'] = public_url
# else:
#     public_url = os.environ['NGROK_URL']

if not os.environ['PHONE_NUMBER']:
    print("Please set a phone number to make calls from.")
else:
    phone_number = os.environ['PHONE_NUMBER']

# ngrok_tunnel_url = os.environ['NGROK_URL']


app = Flask(__name__)


@app.route("/")
def ai_prompt():
    return render_template('/index.html')


if __name__ == '__main__':
    app.run(host="0.0.0.0", port=80)
