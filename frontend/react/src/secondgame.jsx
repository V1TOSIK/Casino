import React, { useEffect, useRef, useState } from "react";

function SecondGame() {
  const canvasRef = useRef(null);
  const [gameOver, setGameOver] = useState(false);
  const [restartKey, setRestartKey] = useState(0); // для перезапуску useEffect

  useEffect(() => {
    const canvas = canvasRef.current;
    const ctx = canvas.getContext("2d");

    const WIDTH = 400;
    const HEIGHT = 600;

    canvas.width = WIDTH;
    canvas.height = HEIGHT;

    let lastMoveTime = 0;
    const MOVE_DELAY = 150; // 150 мс між ходами

    const player = {
      x: WIDTH / 2 - 15,
      y: HEIGHT - 70,
      size: 30,
      color: "yellow",
      speed: 30,
    };

    const lanes = [100, 200, 300, 400];
    const cars = [];
    let spawnInterval;
    let running = true;

    function spawnCar() {
      const lane = lanes[Math.floor(Math.random() * lanes.length)];
      const direction = Math.random() > 0.5 ? 1 : -1;

      cars.push({
        x: direction === 1 ? -100 : WIDTH + 100,
        y: lane,
        width: 50,
        height: 30,
        speed: 2 + Math.random() * 3,
        color: "red",
        direction,
      });
    }

    spawnInterval = setInterval(spawnCar, 300);

    // --- Рух з обмеженням ---
    function handleKey(e) {
      const now = Date.now();
      if (now - lastMoveTime < MOVE_DELAY) return; // блокування спаму
      lastMoveTime = now;

      if (e.key === "w" || e.key === "ArrowUp") player.y -= player.speed;
      if (e.key === "s" || e.key === "ArrowDown") player.y += player.speed;
      if (e.key === "a" || e.key === "ArrowLeft") player.x -= player.speed;
      if (e.key === "d" || e.key === "ArrowRight") player.x += player.speed;

      // --- Блок виходу за межі ---
      if (player.x < 0) player.x = 0;
      if (player.x + player.size > WIDTH) player.x = WIDTH - player.size;
      if (player.y < 0) player.y = 0;
      if (player.y + player.size > HEIGHT) player.y = HEIGHT - player.size;
    }

    window.addEventListener("keydown", handleKey);

    function collides(a, b) {
      return (
        a.x < b.x + b.width &&
        a.x + a.size > b.x &&
        a.y < b.y + b.height &&
        a.y + a.size > b.y
      );
    }

    function resetPlayer() {
      player.x = WIDTH / 2 - 15;
      player.y = HEIGHT - 70;
    }

    function gameLoop() {
      if (!running) return;

      ctx.fillStyle = "#1e293b";
      ctx.fillRect(0, 0, WIDTH, HEIGHT);

      // --- Фініш ---
      ctx.fillStyle = "green";
      ctx.fillRect(0, 0, WIDTH, 40);

      if (player.y <= 10) {
        // Гравець дістався фінішу
        running = false;
        setGameOver(true);
      }

      // Лінії доріг
      lanes.forEach((laneY) => {
        ctx.fillStyle = "#334155";
        ctx.fillRect(0, laneY - 20, WIDTH, 40);
      });

      // Машини
      cars.forEach((car) => {
        car.x += car.speed * car.direction;

        ctx.fillStyle = car.color;
        ctx.fillRect(car.x, car.y - 15, car.width, car.height);

        if (collides(player, car)) {
          resetPlayer();
        }
      });

      // Гравець
      ctx.fillStyle = player.color;
      ctx.fillRect(player.x, player.y, player.size, player.size);

      requestAnimationFrame(gameLoop);
    }

    gameLoop();

    return () => {
      window.removeEventListener("keydown", handleKey);
      clearInterval(spawnInterval);
    };
  }, [restartKey]);

  function restartGame() {
    setGameOver(false);
    setRestartKey((prev) => prev + 1); // перезапуск ефекту
  }

  return (
    <div className="flex flex-col items-center justify-center h-screen text-white">
      <h1 className="text-3xl mb-4">Crossy Road 🐥</h1>

      <canvas ref={canvasRef} style={{ border: "3px solid white" }}></canvas>

      <p className="mt-4 opacity-70">WASD або стрілочки для руху</p>

      {gameOver && (
        <button
          onClick={restartGame}
          className="mt-6 px-6 py-2 bg-green-600 rounded text-xl"
        >
          Почати спочатку
        </button>
      )}
    </div>
  );
}

export default SecondGame;
