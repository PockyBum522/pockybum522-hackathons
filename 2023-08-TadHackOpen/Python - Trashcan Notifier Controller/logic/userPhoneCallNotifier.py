from logic.jsonPoster import JsonPoster


class UserPhoneCallNotifier:
    def __init__(self):
        pass

    @staticmethod
    def notify_jared_with_phone_call():

        json_poster = JsonPoster()

        json_response = json_poster.post_json_to_radisys_api()

        return json_response