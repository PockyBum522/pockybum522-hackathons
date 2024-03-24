from signalwire.voice_response import *
from flask import Flask, request, render_template
import os
from pyngrok import ngrok
from signalwire.rest import Client as signalwireClient
from dotenv import load_dotenv

import os.path
from google.auth.transport.requests import Request
from google.oauth2.credentials import Credentials
from google_auth_oauthlib.flow import InstalledAppFlow
from googleapiclient.discovery import build

import oauth
from email.mime.text import MIMEText
import base64

SECRET_FILE = "client_secret.json"

load_dotenv()

# Set up ngrok tunnel
SIGNALWIRE_PROJECT_KEY = os.environ['SIGNALWIRE_PROJECT_ID']
SIGNALWIRE_TOKEN = os.environ['SIGNALWIRE_API_TOKEN']
SIGNALWIRE_SPACE = os.environ['SIGNALWIRE_SPACE_URL']
SIGNALWIRE_NUMBER = os.environ['SIGNALWIRE_FROM_NUMBER']

swClient = signalwireClient(SIGNALWIRE_PROJECT_KEY, SIGNALWIRE_TOKEN, signalwire_space_url=SIGNALWIRE_SPACE)

ngrok.set_auth_token(os.environ['NGROK_AUTH_TOKEN'])
public_url = ngrok.connect()

app = Flask(__name__)


@app.route("/")
def ai_prompt():
    return render_template('/index.html')


def get_g_service(service="gmail", ver="v1", scopes=['https://www.googleapis.com/auth/gmail.readonly',
                                                     'https://www.googleapis.com/auth/gmail.send']):
    creds = None
    if os.path.exists('token.json'):
        creds = Credentials.from_authorized_user_file('token.json', scopes)
    # If there are no (valid) credentials available, let the user log in.
    if not creds or not creds.valid:
        if creds and creds.expired and creds.refresh_token:
            creds.refresh(Request())
        else:
            flow = InstalledAppFlow.from_client_secrets_file(SECRET_FILE, scopes)
            creds = flow.run_local_server(port=8080)
        # Save the credentials for the next run
    with open('token.json', 'w') as token:
        token.write(creds.to_json())
    return build(service, ver, credentials=creds)


def create_message(sender, to, subject, message_text):
    message = MIMEText(message_text)
    message['to'] = to
    message['from'] = sender
    message['subject'] = subject
    return {'raw': base64.urlsafe_b64encode(message.as_string().encode()).decode()}


def send_message(sender, to, subject, message_text, user_id='me'):
    msg = create_message(sender, to, subject, message_text)
    try:
        service = get_g_service()
        message = (service.users().messages().send(userId=user_id, body=msg)
                   .execute())
        print('Message Id: %s' % message['id'])
        return message
    except ValueError as e:
        print('An error occurred: %s' % e)


if __name__ == '__main__':
    send_message("14074632925@tmomail.net", "pockybum522@gmail.com",
                 "Important! Grandma Lisa had a high blood sugar reading of 130 as of March 24th, 2024", "",
                 user_id='me')

    # Show ngrok URL and tunnel
    # print()
    # print(public_url)
    # print()
    #
    # # This should work once
    #
    # print()
    # print()
    # print('Sending message using ph: ' + SIGNALWIRE_NUMBER)
    # print()
    # print()
    #
    # success = swClient.messages.create(to='+14074632925', from_=SIGNALWIRE_NUMBER,
    #                                    body='Important! Grandma Lisa had a high blood sugar reading of 130 as of March 24th, 2024')

    # Run flask server
    # app.run(host="0.0.0.0", port=80)
