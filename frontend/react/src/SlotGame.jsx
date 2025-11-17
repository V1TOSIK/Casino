import React, { useState, useRef, useEffect } from "react";

const symbolsList = ['🍒', '🍋', '🍉', '🍊', '⭐', '7️⃣', '🔔'];
const symbolHeight = 110;
const visibleSymbols = 30;
const rows = 3;
const cols = 3;

function App() {
  const [balance, setBalance] = useState(1000); // Початковий баланс 1000 монет
  const [bet, setBet] = useState(10);
  const [spinsCount, setSpinsCount] = useState(1);
  const [resultText, setResultText] = useState('');
  const [isSpinning, setIsSpinning] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false); // Стан для модального вікна

  const currentPositions = useRef(new Array(rows * cols).fill(0));
  const reelsRefs = useRef([]);

  const animateReel = (element, from, to, duration, onComplete) => {
    const totalSymbols = symbolsList.length;
    const distance = to - from;
    const startTime = performance.now();

    function update(currentTime) {
      const elapsed = currentTime - startTime;
      const progress = Math.min(elapsed / duration, 1);
      const eased = 1 - Math.pow(1 - progress, 3);
      const position = from + distance * eased;
      const offset = -(position % totalSymbols) * symbolHeight;
      element.style.transform = `translateY(${offset}px)`;

      if (progress < 1) requestAnimationFrame(update);
      else onComplete();
    }

    requestAnimationFrame(update);
  };

  const processResults = (allResults, betAmount) => {
    let totalWin = 0;

    allResults.forEach(results => {
      const grid = [];
      for (let r = 0; r < rows; r++) {
        grid[r] = results.slice(r * cols, r * cols + cols);
      }

      // Горизонталі
      for (let r = 0; r < rows; r++) {
        if (grid[r][0] === grid[r][1] && grid[r][1] === grid[r][2]) {
          totalWin += betAmount * 3;
        }
      }

      // Вертикалі
      for (let c = 0; c < cols; c++) {
        if (grid[0][c] === grid[1][c] && grid[1][c] === grid[2][c]) {
          totalWin += betAmount * 4;
        }
      }

      // Діагоналі
      if (grid[0][0] === grid[1][1] && grid[1][1] === grid[2][2]) totalWin += betAmount * 2;
      if (grid[0][2] === grid[1][1] && grid[1][1] === grid[2][0]) totalWin += betAmount * 2;
    });

    if (totalWin > 0) {
      setResultText(`🎉 Виграш за ${allResults.length} спін(ів): +${totalWin} монет!`);
    } else {
      setResultText('😢 Нічого, спробуйте ще!');
    }

    // Оновлення балансу
    setBalance(prevBalance => prevBalance + totalWin);
  };

  const startSpin = () => {
    if (isSpinning) return;
    if (bet <= 0) return alert("Введіть правильну ставку!");
    if (spinsCount <= 0) return alert("Виберіть кількість спінів!");
    if (bet * spinsCount > balance) return alert("Недостатньо балансу!");

    // Оновлення балансу на момент початку гри
    setBalance(prevBalance => prevBalance - bet * spinsCount); // Віднімаємо ставку з балансу
    setIsSpinning(true);
    setResultText('');

    let spinResults = [];
    let completedSpins = 0;

    const spinOnce = (spinIndex) => {
      const results = new Array(rows * cols);
      let reelsCompleted = 0;

      reelsRefs.current.forEach((reel, index) => {
        const totalSymbols = symbolsList.length;
        const randomIndex = Math.floor(Math.random() * totalSymbols);
        const fullSpins = 3 + index;
        const targetPos = currentPositions.current[index] + fullSpins * totalSymbols + randomIndex;

        animateReel(reel.querySelector('.symbols'), currentPositions.current[index], targetPos, 2000 + index * 200, () => {
          currentPositions.current[index] = targetPos % totalSymbols;
          results[index] = symbolsList[currentPositions.current[index]];
          reelsCompleted++;
          if (reelsCompleted === reelsRefs.current.length) {
            spinResults.push(results);
            completedSpins++;
            if (completedSpins < spinsCount) spinOnce(completedSpins);
            else {
              processResults(spinResults, bet);
              setIsSpinning(false);
            }
          }
        });
      });
    };

    spinOnce(0);
  };

  const handleDeposit = () => {
    // Поповнення балансу
    setBalance(1000); // Встановлюємо баланс на 1000 монет
    setIsModalOpen(false); // Закриваємо модальне вікно
  };

  useEffect(() => {
    if (balance <= 0) {
      // Якщо баланс 0, показуємо модальне вікно
      setIsModalOpen(true);
    }
  }, [balance]);

  return (
    <div className="h-screen w-screen flex justify-center items-center bg-gradient-to-b from-slate-900 to-slate-800">
      <div className="relative z-10 bg-gray-800 p-8 rounded-2xl shadow-2xl flex flex-col items-center w-full max-w-lg mx-4">
        {/* Баланс */}
        <div className="mb-4 text-lg">Баланс: <span>{balance}</span> монет</div>

        {/* Панель ставок */}
        <div className="mb-6 flex gap-4 items-center flex-wrap justify-center w-full">
          <label className="text-lg">Ставка:</label>
          <input
            type="number"
            min="1"
            value={bet}
            onChange={(e) => setBet(Number(e.target.value))}
            className="border rounded p-2 w-24 text-black"
          />

          <label className="text-lg">Кількість спінів:</label>
          <select
            value={spinsCount}
            onChange={(e) => setSpinsCount(Number(e.target.value))}
            className="border rounded p-2 w-24 bg-white text-black"
          >
            {Array.from({ length: 10 }, (_, i) => i + 1).map(num => (
              <option key={num} value={num}>{num}</option>
            ))}
          </select>
        </div>

        {/* Слоти */}
        <div className="grid grid-cols-3 gap-6 mb-6">
          {Array.from({ length: rows * cols }).map((_, index) => (
            <div
              key={index}
              className="reel relative w-[110px] h-[110px] rounded-xl bg-white shadow-lg overflow-hidden"
              ref={el => reelsRefs.current[index] = el}
            >
              <div className="symbols absolute top-0 left-0 w-full flex flex-col h-[3300px]">
                {Array.from({ length: visibleSymbols }).map((_, j) => (
                  <div key={j} className="symbol h-[110px] flex items-center justify-center text-4xl">
                    {symbolsList[j % symbolsList.length]}
                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>

        {/* Результат */}
        <div className="text-2xl font-semibold mt-4 h-8 mb-6 text-center">
          {resultText}
        </div>

        {/* Кнопка SPIN */}
        <button
          onClick={startSpin}
          className="bg-gradient-to-br from-yellow-400 to-yellow-600 text-white font-bold text-xl px-12 py-4 rounded-full shadow-[0_5px_0_#92400e] active:translate-y-[3px] active:shadow-[0_2px_0_#78350f] active:bg-gradient-to-br active:from-yellow-600 active:to-yellow-800 transition-all duration-150 cursor-pointer select-none w-full sm:w-auto"
        >
          SPIN 🎯
        </button>
      </div>

      {/* Модальне вікно поповнення рахунку */}
      {isModalOpen && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
          <div className="bg-cyan p-8 rounded-lg shadow-xl text-center">
            <h2 className="text-xl font-semibold mb-4">Ваш баланс закінчився!</h2>
            <p className="mb-6">Хочете поповнити рахунок на 1000 монет?</p>
            <div className="flex justify-center gap-4">
              <button
                onClick={handleDeposit}
                className="bg-green-500 text-yelow px-6 py-2 rounded-lg hover:bg-green-600"
              >
                Так, поповнити
              </button>
              <button
                onClick={() => setIsModalOpen(false)}
                className="bg-red-500 text-green px-6 py-2 rounded-lg hover:bg-red-600"
              >
                Ні, пізніше
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default App;
