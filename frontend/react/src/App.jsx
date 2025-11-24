import React, { useState } from "react";
import SlotGame from "./SlotGame"; 
import SecondGame from "./secondgame";

function MainApp() {
  const [screen, setScreen] = useState("slot");

  return (
    <div className="h-screen w-screen bg-slate-900 text-white">
      <div className="flex gap-4 p-4">
        <button
          onClick={() => setScreen("slot")}
          className="px-4 py-2 bg-yellow-600 rounded"
        >
          Слоти
        </button>
        <button
          onClick={() => setScreen("crossy")}
          className="px-4 py-2 bg-green-600 rounded"
        >
          Crossy Road
        </button>
      </div>

      {screen === "slot" && <SlotGame />}
      {screen === "crossy" && <SecondGame />}
    </div>
  );
}

export default MainApp;
