
  // This file was automatically generated. Edits will be overwritten

  export interface Typegen0 {
        '@@xstate/typegen': true;
        internalEvents: {
          "xstate.init": { type: "xstate.init" };
        };
        invokeSrcNameMap: {
          
        };
        missingImplementations: {
          actions: "rateBeer";
          delays: never;
          guards: never;
          services: never;
        };
        eventsCausingActions: {
          "getRandomBeer": "BUTTON_PRESSED";
"rateBeer": "RATING_SENT";
        };
        eventsCausingDelays: {
          
        };
        eventsCausingGuards: {
          
        };
        eventsCausingServices: {
          
        };
        matchesStates: "beerOnScreen" | "loaded" | "loading" | "unloaded";
        tags: never;
      }
  