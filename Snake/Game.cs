using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class Game						// класс Game, обрабатывает всю логику игры и занимается покадровой отрисовкой
	{
        public Vector2u SizeField => new Vector2u(snakeTexture.Size.X * (uint)size.X, snakeTexture.Size.Y * (uint)size.Y);	// размер игрового поля: переиножаем длину и ширину одного тайла на размер матрицы
        public bool GameOver => snake.IsCollide;        // если было столкновение с самим собой, то конец игры

		private Font    font;				// шрифт
        private Texture gameOverTexture;	// текстура надписи "Game Over"
        private Texture snakeTexture;		// текстура змеи
        private Texture fruitTexture;		// текстура фрукта
        private Texture fieldTexture;       // текстура поля

		private Text    scoreText;			// текст игрового счёта
        private Sprite  gameOver;           // спрайт надписи "Game Over"
		private Sprite  field;				// прайт игрового поля
        private Fruit   fruit;				// фрукт
        private Snake   snake;				// змея

        private Vector2i size;				// размер матрицы игрового поля

        private Clock clock;				// часы
        private float timer;				// накопитльный счётчик времени
        private float delay;				// скорость обновления змеи
        private int score;                  // игровой счёт

		public Game(Vector2i size)          // в качестве параметра выступает размер матрицы игрового поля
		{
            this.size = size;

			// загружаем ресурсы
			font			= new Font("fonts/imagine.ttf");
            gameOverTexture = new Texture("images/gameover.png");
            snakeTexture    = new Texture("images/snakes.png");
            fruitTexture    = new Texture("images/fruit.png");
            fieldTexture    = new Texture("images/field.png");

			// инициализируем игровые объекты
			scoreText	= new Text("Score: 0", font)
            {
                Position = new Vector2f(10,0),
                Color = Color.Black
            };
            gameOver    = new Sprite(gameOverTexture)
            {
                Color = Color.Black
            };
            field       = new Sprite(fieldTexture);
            fruit       = new Fruit(size)
            {
                Texture = fruitTexture
            };
            snake       = new Snake(new Vector2i(10,10))
            {
                Texture = snakeTexture
            };

			// подписываемся на событие, когда змея съедает фрукт
			snake.EatFruit += OnSnakeEatFruit;

            clock = new Clock();

			// устанавливаем значения по умолчанию
			timer = 0;
            delay = 0.1f;
            score = 0;
        }

        public void OnWindowKeyPressed(object sender, KeyEventArgs e)   // обработчик нажатия клавиши
		{
            if(e.Code == Keyboard.Key.R)    // если клавиша R
			{
                if (GameOver)               // если конец игры
					Restart();              // перезапускаем игру
			}
        }

        public void Update()                // обновление игровой логики
		{
            if (GameOver) return;           // если конец игры, то выходим

			float time = clock.ElapsedTime.AsSeconds(); // получаем текущее время в секундах
			clock.Restart();                            // сбрасываем часы
			timer += time;                              // накапливаем в счётчике прошедшее время

			if (timer > delay)                          // если накопившееся время больше времени времени на одно обновление
			{
                timer = 0;                              // брасываем счётчик
				snake.Update(size, fruit);              // обновляем логику змеи, передавая в качестве параметра размер матрицы игрового поля и ссылку на фрукт
			}
        }

        public void Restart()               // рестарт игры
		{
            snake.Restart(new Vector2i(10, 10));    // рестартим змею с указанием стартовой позиции
			fruit.Restart(size);                    // рестартим фрукт с указанием размера матрицы игрового поля
			score = 0;
			scoreText.DisplayedString = "Score: " + score.ToString();
		}

        public void Render(RenderTarget target) // рендер игры
		{
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)    // пробегаемся по координатам X и Y
				{
                    field.Position = new Vector2f(				// устанавливаем позицию плитки
                        i * field.GetGlobalBounds().Width,		// по координате X
                        j * field.GetGlobalBounds().Height);    // по координате Y
																// домножая номер клетки матрицы на ширину и высоту одной плитки соответственно
					target.Draw(field);                         // отрисовываем плитку
																// и так для каждой
				}
			}

            target.Draw(snake);						// отрисовка змеи
            target.Draw(fruit);						// отрисовка фрукта
            target.Draw(scoreText);                 // отрисовка текста

			if (GameOver)                           // если конец игры
				target.Draw(gameOver);              // отрисовываем надпись Game Over
		}

        private void OnSnakeEatFruit(object sender, EventArgs e)    // обработка события, когда змея съедает фрукт
		{
            score++;                                                    // увеличиваем счёт
			scoreText.DisplayedString = "Score: " + score.ToString();   // обновляем надпись
		}
    }
}
