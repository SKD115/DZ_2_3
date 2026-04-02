using System;
using System.Collections.Generic;

namespace ConsoleApp2.ocp
{
    class Case1
    {
        interface ICoolGuy
        {
            void CallCoolGuy();
        }
        // ЧТО: Добавили интерфейс расширения отрисовки пользователя
        // ЗАЧЕМ: Чтобы новые сценарии рисования добавлять через новые классы, не меняя основной метод User.DrawUser(), соблюдая OCP
        interface IUserDrawExtension
        {
            bool CanDraw(User user);
            void Draw(User user);
        }

        class User
        {
            private readonly bool _isSelected;
            private readonly string _image;
            // ЧТО: Добавили патерн для разных ситуаций
            // ЗАЧЕМ: Теперь для редких/специальных случаев не нужно изменять User
            private readonly IEnumerable<IUserDrawExtension> _drawExtensions;

            public User(bool isSelected, string image, IEnumerable<IUserDrawExtension> drawExtensions = null)
            {
                _isSelected = isSelected;
                _image = image;
                _drawExtensions = drawExtensions ?? new List<IUserDrawExtension>();
            }

            public void DrawUser()
            {
                if (_isSelected)
                    DrawEllipseAroundUser();

                if (_image != null)
                    DrawImageOfUser();
                // ЧТО: Убрали проверку "if (this is ICoolGuy)" и заменили проверку патернов
                // ЗАЧЕМ:Раньше для каждого нового патерна пришлось бы менять DrawUser(). Теперь новые патерны просто подключаются как новые IUserDrawExtension
                foreach (var extension in _drawExtensions)
                {
                    if (extension.CanDraw(this))
                        extension.Draw(this);
                }
            }
            // ЧТО: Добавили свойства только для чтения.
            // ЗАЧЕМ: Чтобы патерны могли безопасно получать данные пользователя,  не пытаясь взять данные из приватных полей напрямую
            public bool IsSelected => _isSelected;
            public string Image => _image;

            void DrawEllipseAroundUser() { }
            void DrawImageOfUser() { }
            // ЧТО: Сделали метод internal, чтобы патерн мог вызвать нужную отрисовку
            // ЗАЧЕМ: Логика рисования очков остаётся внутри User, но решение КОГДА рисовать вынесено наружу
            internal void DrawCoolGuyGlasses() { }
        }
        // ЧТО: Специальный случай "крутой парень" вынесен в отдельный патерн
        // ЗАЧЕМ: Теперь эта логика расширяет систему без изменения класса User
        class CoolGuyDrawExtension : IUserDrawExtension
        {
            public bool CanDraw(User user)
            {
                return user is ICoolGuy;
            }

            public void Draw(User user)
            {
                user.DrawCoolGuyGlasses();
            }
        }

        class CoolUser : User, ICoolGuy
        {
            public CoolUser(bool isSelected, string image, IEnumerable<IUserDrawExtension> drawExtensions = null)
                : base(isSelected, image, drawExtensions)
            {
            }

            public void CallCoolGuy()
            {
                // бла бла логика
            }
        }
    }
}
