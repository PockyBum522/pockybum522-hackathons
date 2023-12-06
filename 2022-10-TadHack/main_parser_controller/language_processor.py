import re


def get_address_from_transcription(conversation):

    m = re.search(r'my address is (.+ \d{5})', conversation, re.IGNORECASE)
    capture = m.group(1)

    return capture


def get_customer_name_from_transcription(conversation):

    m = re.search(r'my name is (\w+.\w+)', conversation, re.IGNORECASE)
    capture = m.group(1)

    return capture


def get_appt_time_from_transcription(conversation):

    m = re.search(r'time is (\d{2} p\.m\.)', conversation, re.IGNORECASE)
    capture = m.group(1)

    return capture
