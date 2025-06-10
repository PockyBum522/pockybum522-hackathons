namespace NotesServer;

public class ExampleData
{
  public static string ExampleOpenAIJsonResponse = """
                                                   {
                                                     "id": "chatcmpl-BggpLQYHwrwBR9nngI3AHzDOHrwTX",
                                                     "object": "chat.completion",
                                                     "created": 1749514103,
                                                     "model": "gpt-4o-2024-08-06",
                                                     "choices": [
                                                       {
                                                         "index": 0,
                                                         "message": {
                                                           "role": "assistant",
                                                           "content": "P/NO. SN20P34496     FRU NO. 01YP680\nLCFC P/N  PK131671B00\nSG-90850-XUA  01  NUM-BL US       SN5372BL\n8SSN20P34496L1CZ9CM00J9  C2  MADE IN CHINA",
                                                           "refusal": null,
                                                           "annotations": []
                                                         },
                                                         "logprobs": null,
                                                         "finish_reason": "stop"
                                                       }
                                                     ],
                                                     "usage": {
                                                       "prompt_tokens": 810,
                                                       "completion_tokens": 73,
                                                       "total_tokens": 883,
                                                       "prompt_tokens_details": {
                                                         "cached_tokens": 0,
                                                         "audio_tokens": 0
                                                       },
                                                       "completion_tokens_details": {
                                                         "reasoning_tokens": 0,
                                                         "audio_tokens": 0,
                                                         "accepted_prediction_tokens": 0,
                                                         "rejected_prediction_tokens": 0
                                                       }
                                                     },
                                                     "service_tier": "default",
                                                     "system_fingerprint": "fp_a288987b44"
                                                   }

                                                   """;
}