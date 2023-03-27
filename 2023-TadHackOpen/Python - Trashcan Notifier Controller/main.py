import os

import werkzeug

from flask import Flask
from logic.flaskProductionServerWarningDisabler import flask_warning_suppressor

app = Flask(__name__)

from endpoints import flaskEndpoints

if __name__ == "__main__":
    print("")
    print(" * Starting trashcan notification controller...")
    print("")

    # noinspection SpellCheckingInspection
    os.environ["PYTHONUNBUFFERED"] = "false"

    werkzeug.serving._ansi_style = flask_warning_suppressor(werkzeug.serving._ansi_style)

    app.run(host='0.0.0.0', port=5000, debug=True)
