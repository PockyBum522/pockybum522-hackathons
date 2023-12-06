# This is a sample Python script.
import time
import simple_websocket
from flask import Flask, request, Response, json
import language_processor
import logging
import click

app = Flask(__name__)

cumulative_transcription = ""

#cumulative_transcription = "how are you doing hello I'm doing absolutely fantastic how are you doing I am doing well I would like to get my computer repaired do you do that you snow I absolutely do that that's one of the best things that I do extra my name is David Sykes my address is 460 Allen Street Winter Park Florida 32789when is your next available time is 12 p.m. excellent I will see you at 12 p.m. I look forward to getting a computer repair goodbye goodbye"


# To keep flask from printing to the console for demo
def secho(text, file=None, nl=None, err=None, color=None, **styles):
    pass


def echo(text, file=None, nl=None, err=None, color=None, **styles):
    pass


click.echo = echo
click.secho = secho
# END keep flask from printing to the console for demo

@app.route('/calling-main-controller', methods=['POST'])
def main_controller_call_test():
    data = request.json

    # print("calling-main-controller DATA FROM JAMBONZ:")
    # print(json.dumps(data, indent=1))
    # print()

    post_payload = json.dumps(
        [{"verb": "pause", "length": 1.5},
         {"verb": "say", "text": "<speak>You may now begin your call.</speak>"},
         {"verb": "listen",
          "mixType": "stereo",
          "actionHook": "http://pockybum522.com/transcription-hook",
          "url": "http://pockybum522.com/audio-sockets",
          "transcribe": {
                "transcriptionHook": "http://pockybum522.com/transcription-hook",
                "recognizer": {
                    "vendor": "google",
                    "language": "en-US",
                    "interim": False,
                    "separateRecognitionPerChannel": True
                }
             }
          },
         {"verb": "pause", "length": 240}])

    return Response(post_payload, mimetype="application/json")


@app.route('/calling-main-controller-status', methods=['POST'])
def main_controller_call_status_test():
    global cumulative_transcription
    data = request.json

    call_current_status = data["call_status"]

    if call_current_status == "completed":
        print()
        print("Full Transcription of conversation: ")
        print(cumulative_transcription)
        print()

        addr_from_call = language_processor.get_address_from_transcription(cumulative_transcription)
        appt_time_from_call = language_processor.get_appt_time_from_transcription(cumulative_transcription)
        customer_name_from_call = language_processor.get_customer_name_from_transcription(cumulative_transcription)

        print("Customer name detected from customer is: ", customer_name_from_call)
        print("Address detected from customer is: ", addr_from_call)
        print("Appointment time detected from customer is: ", appt_time_from_call)
        print()

        print("Now creating calendar event for %s at %s...Done " % (customer_name_from_call, appt_time_from_call))
        print()

        print("Now creating Android contact for %s including their address: %s...Done" %
              (customer_name_from_call, addr_from_call))
        print()

        print("Now creating summary SMS of actions completed...Sending. ")
        print()

        print("Conversation actions completed. ")
        print()

    return data


@app.route('/transcription-hook', methods=['POST'])
def main_controller_transcription_hook():
    global cumulative_transcription

    data = request.json

    speech_transcript = ""

    try:
        speech = data["speech"]
        alternatives = speech["alternatives"]
        first_alternative = alternatives[0]
        speech_transcript = first_alternative["transcript"]
    except KeyError:
        # It won't have the speech key for the call after the user hangs up
        pass

    cumulative_transcription += speech_transcript

    # print("Transcription so far:")
    # print(cumulative_transcription)
    # print()

    return data


# This just throws away data so Jambonz has something to connect to and send audio even though all we care about
# is getting the transcription later
@app.route('/audio-sockets', websocket=True)
def status_socket():
    ws = simple_websocket.Server(request.environ)
    try:
        while ws.connected:
            time.sleep(.5)
    except simple_websocket.ConnectionClosed:
        # print("ConnectionClosed from websockets")
        pass
    return ''


@app.route('/')
def hello():
    return 'Webhooks with Python'


if __name__ == '__main__':
    #cumulative_transcription
    #cumulative_transcription = "how are you doing hello I'm doing absolutely fantastic how are you doing I am doing well I would like to get my computer repaired do you do that you snow I absolutely do that that's one of the best things that I do extra my name is David Sykes my address is 460 Allen Street Winter Park Florida 32789when is your next available time is 12 p.m. excellent I will see you at 12 p.m. I look forward to getting a computer repair goodbye goodbye"

    log = logging.getLogger('werkzeug')
    log.setLevel(logging.ERROR)
    log.disabled = True

    app.run(debug=False, host="0.0.0.0")