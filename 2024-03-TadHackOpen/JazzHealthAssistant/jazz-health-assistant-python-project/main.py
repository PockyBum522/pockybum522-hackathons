from signalwire.voice_response import *
from flask import Flask, request, render_template
import os
from pyngrok import ngrok

# Set up ngrok tunnel
ngrok.set_auth_token(os.environ['NGROK_AUTH_TOKEN'])
public_url = ngrok.connect()

app = Flask(__name__)


@app.route("/")
def ai_prompt():
    return render_template('/index.html')


if __name__ == '__main__':
    # Show ngrok URL and tunnel
    print()
    print(public_url)
    print()

    # Run flask server
    app.run(host="0.0.0.0", port=80)
