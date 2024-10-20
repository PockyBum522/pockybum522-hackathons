# The Shrine 

## A huge thank you to: 

The sponsors, without whom TadHack would not be possible, and Alan (also without whom TadHack would not be possible) for putting on TadHack, and check the bottom of this document for credit to projects that were used in the making of The Shrine

## Description

...is an attempt at a secular yet spritual experience. We affectionately refer to it as being at the imaginary Temple of Computing.

The hack works as follows:

A parishioner is invited to "The Shrine" by a Technosage. The parishioner is given a 3D-printed coin that unbeknownst to them has a NFC tag that can store data embedded on it.

The parishioner then steps up to The Shrine and their presence is detected by a time of flight sensor which then signals the server to begin the experience.

The server sends events as a JSON array to the frontend which is responsible for showing the different steps of the experience on a projector.

The frontend then invites the parishioner to contemplate their hopes, dreams, and fears as a set of slides. 

The frontend then invites the parishioner to place their coin on the altar, beneath which is a NFC reader attached to the server that detects the presence of the coin.

The server, once having detected a coin, stores the ID of the NFC tag in the coin and then sends an event to the frontend so that a slide asking the parishioner to speak on their contemplations can be shown.

The server then starts voice recognition and records the spoken contemplations of the parishioner.

These words are then sent to ChatGPT for sentiment analysis, to be used later. ChatGPT is also used to get the three most emotionally charged words that were spoken, and is also used to clean up and fix any errors in the speech to text transcription.

The sentiment analysis, cleaned full transcript, and top three most emotionally charged words are sent to the frontend in a VCon so that the frontend can use this data to adjust the visual effects projected into the shrine to match the sentiment of the user. The three most emotionally charged words aquired from ChatGPT are sent as Body messages in an Attachment property of the VCon, which could also be used by the frontend for visual effect.

At this point, the parishioner is thanked for visiting the shrine in the last slide shown by the frontend. The parishioner then leaves The Shrine.

As they leave, The Technosage instructs them that they may later want to tap the coin to their phone and reflect on their contemplations at The Shrine.

The parishioner, whenever they choose to do this, will find that the server has generated a static site/webpage with their transcript and visual effects on the page that are based on ChatGPT's sentiment analysis. Once the server generates this, it is uploaded to hosting which is how the parishioner can access it with the coin, as when they placed the coin on the altar, the server recorded the ID of the NFC tag that it saw so that it can upload the associated transcription that was spoken when that particular coin was on the altar, for later  reflection by the parishioner. 

# Thank you to the following projects
(which were used in our project)

XState:
https://xstate.js.org/

Coin model:
https://www.thingiverse.com/thing:5695821

Speech to text:
https://github.com/pengzhendong/streaming-sensevoice/

(NFC reader cover) Altar model:
https://www.thingiverse.com/thing:3469588

ManagedLibNFC:
https://github.com/kgamecarter/ManagedLibnfc