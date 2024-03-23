from signalwire.voice_response import *
from flask import Flask, request, render_template
import sqlite3
import os
import logging
from pyngrok import ngrok

if not os.environ['NGROK_URL']:
    public_url = ngrok.connect(5000)
    os.environ['NGROK_URL'] = public_url
else:
    public_url = os.environ['NGROK_URL']

if not os.environ['PHONE_NUMBER']:
    print ("Please set a phone number to make calls from.")
else:
    phone_number = os.environ['PHONE_NUMBER']


ngrok_tunnel_url = os.environ['NGROK_URL']


app = Flask(__name__)

@app.route("/", methods=["POST"])
def ai_prompt():
    ## AI ZIPLINK AGENT ##
    swml_web_hook_base_url = ngrok_tunnel_url
    zip_code_table = {
"90210":"+13105550123",
"10001":"+12125550123"
}

    swml_ai_prompt = '''Your name is Zippy an AI Assistant for ZipLink.  Your job is to transfer the caller to the proper office depending on their location determined by ZIP codes.

    ### How to follow up on questions answered and protocols to follow
        Stay on focus and on protocol.
        You are not capable of troubleshooting or diagnosing problems.
        Execute functions when appropriate
        Only allow responses to be five digit numerical United States ZIP codes.

    ### Step 1
        Introduce yourself as Zippy and inform the user that you will be assisting them by transferring to their local office.

    ### Step 2
        Ask the caller to provide their ZIP code to determine which office they should be transferred to.  The zip code must be 5 digits in length and must be numerical in order to be valid.

    ### Step 3
        Use the callers zip code to find the office phone number in the meta data table of the transfer function.  Transfer the caller to that destination.
    '''

    swml = {}
    swml['sections'] = {
        'main': [{
            'ai': {
                'languages': [
                    {
                        "engine": "elevenlabs",
                        "fillers": [
                            "ok",
                            "thanks"
                        ],
                        "name": "English",
                        "code": "en-US",
                        "voice": "josh"
                    }
                ],
                'params': {
                    'confidence': 0.6,
                    'barge_confidence': 0.1,
                    'top_p': 0.3,
                    'temperature': 1.0,
                    'openai_gcloud_version': "gcloud_speech_v2_async",
                    'swaig_allow_swml': True,
                    'conscience': True
                },
                'prompt': {
                    'text': swml_ai_prompt
                },
                'SWAIG': {
                    'functions': [
                        {
                            'function': 'transfer',
                            'purpose': 'transfer the caller to the correct number based on zip code',
                            'data_map': {
                              'expressions': [
                                {
                                  'output': {
                                    'action': [
                                      {
                                        'SWML': {
                                          'version': '1.0.0',
                                          'sections': {
                                          'main': [
                                            {
                                              'connect': {
                                                'to': '${meta_data.table.${lc:args.zipcode}}',
                                                'from': phone_number
                                              }
                                            }
                                          ]
                                        }
                                      },
                                      'transfer': 'true'
                                    }
                                  ],
                                  'post_process': 'true',
                                  'response': 'Tell the user that you are transferring the call to their local office'
                                  },
                                  'pattern': '\\d{5}',
                                  'string': "${meta_data.table.${lc:args.zipcode}}"
                                },
                                {
                                  'pattern': '.*',
                                  'pattern': '\\d{5}',
                                  'string': '${meta_data.table.${lc:args.zipcode}}'
                                },
                                {
                                  'pattern': '.*',
                                  'output': {
                                  'response': 'Im sorry, it appears that zip code is not within our coverage area at this time.'
                                },
                                'string': '${args.zipcode}'
                                }
                              ]
                            },
                            'meta_data': {
                              'table': zip_code_table
                            },
                            'argument': {
                              'type': 'object',
                              'properties': {
                                  'zipcode': {
                                      'type': 'string',
                                      'description': 'the callers zip code'
                                  }
                              }
                           }
                        }
                    ]
                }
            }
        }]
    }

    return (swml)



if __name__ == '__main__':
    app.run(host="0.0.0.0", port=5000)
