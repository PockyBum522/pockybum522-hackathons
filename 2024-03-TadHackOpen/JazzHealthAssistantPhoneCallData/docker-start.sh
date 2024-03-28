#!/bin/bash

/usr/local/bin/ngrok config add-authtoken $NGROK_TOKEN > /dev/null 2>&1
exec bash
