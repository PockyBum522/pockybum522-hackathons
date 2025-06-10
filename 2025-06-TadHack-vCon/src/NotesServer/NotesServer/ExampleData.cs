namespace NotesServer;

public class ExampleData
{
  public static string ExampleVcon = """
                                     {
                                       "uuid": "01975767-27e2-8620-9dd8-dd37220d739c",
                                       "vcon": "0.0.1",
                                       "created_at": "2025-06-10T01:14:27.682416+00:00",
                                       "redacted": {},
                                       "group": [],
                                       "parties": [
                                         {
                                           "name": "OpenAI",
                                           "role": "LLM"
                                         }
                                       ],
                                       "dialog": [
                                         {
                                           "type": "text",
                                           "start": "2025-06-10T01:14:27.682967+00:00",
                                           "parties": [
                                             0
                                           ],
                                           "mimetype": "text/plain",
                                           "body": "P/NO. SN20P34496     FRU NO. 01YP680\nLCFC P/N  PK131671B00\nSG-90850-XUA  01  NUM-BL US"
                                         }
                                       ],
                                       "attachments": [
                                         {
                                           "type": "tags",
                                           "body": [
                                             "location:28.644655, -81.465546"
                                           ],
                                           "encoding": "json"
                                         }
                                       ],
                                       "analysis": []
                                     }
                                     """;
    
  public static string ExampleOpenAiJsonResponse = """
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