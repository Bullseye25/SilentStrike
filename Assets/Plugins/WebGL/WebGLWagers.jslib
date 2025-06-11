var FrontEndLib = {
  SendToFrontEnd: function (message) {
    var jsMessage = UTF8ToString(message);
    console.log("Message from Unity:", jsMessage);

    // Check if unityCallback is defined before calling
    if (typeof unityCallback === "function") {
      unityCallback(jsMessage);
    } else {
      console.warn("unityCallback is not defined.");
    }

    // Optional: dispatch a custom event if you listen to it elsewhere
    window.dispatchEvent(new Event("unityMessageReceived"));
  },

  GetLeaderboardFromDatabase: function () {
    console.log("JsLib: Fetching Leaderboard Data");

    var url =
      "https://minigame-backend-private-git-feat-express-backend-game-deploy.vercel.app/leaderboard/GetLeaderboard";

    fetch(url)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((data) => {
        console.log("Leaderboard Data:", data);
        var jsonData = JSON.stringify(data);

        if (typeof myGameInstance !== "undefined") {
          myGameInstance.SendMessage(
            "Leaderboard",
            "CallbackGetLeaderboard",
            jsonData
          );
        } else {
          console.warn("myGameInstance is not defined");
        }
      })
      .catch((error) => {
        console.error("Error fetching leaderboard data:", error);
      });
  },
};

mergeInto(LibraryManager.library, FrontEndLib);
