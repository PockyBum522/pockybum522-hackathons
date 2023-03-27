import os
import werkzeug

from flask import Flask
from logic.flaskProductionServerWarningDisabler import flask_warning_suppressor

if __name__ == "__main__":

    print("")
    print(" * Starting trashcan notification controller...")
    print("")

    # noinspection SpellCheckingInspection
    os.environ["PYTHONUNBUFFERED"] = "false"

    werkzeug.serving._ansi_style = flask_warning_suppressor(werkzeug.serving._ansi_style)

    app = Flask(__name__)

    app.run(host='0.0.0.0', port=5000, debug=True)
