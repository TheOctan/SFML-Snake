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
    class Program
    {
        public static RenderWindow window;
        public static Game game;
        public static Random random;

        static void Main(string[] args)
        {
            random = new Random();
            game = new Game(new Vector2i(30, 20));					// создаём экземляр класс Game с размером поля 30 на 20 клеток

            window = new RenderWindow(new VideoMode(game.SizeField.X, game.SizeField.Y), "Snake", Styles.Close);    // создаём окно размером 400x533 с названием Snake
            window.SetMouseCursorVisible(false);                                                                    // делаем невидиомой курсор мыши

            Image icon = new Image("images/Snake.png");				// загружаем
            window.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);  // и устанавливаем иконку на окно

            window.Closed += OnWindowClosed;                        // подписываемся на событие закрития окна
			window.KeyReleased += game.OnWindowKeyPressed;			// подписываем класс Game на событие нажатия кнопки

            while (window.IsOpen)				// бесконечный цикл пока открыто окно
            {
                window.DispatchEvents();		// обрабатываем события

                game.Update();                  // обновляем логику игры

				window.Clear();                 // очищаем экран		
				game.Render(window);            // рендерим игру
				window.Display();               // отображаем на дисплее
			}
        }

        private static void OnWindowClosed(object sender, EventArgs e)  // обработчик события закрытия окна
		{
            window.Close();     // просто закрываем окно
		}
    }
}
