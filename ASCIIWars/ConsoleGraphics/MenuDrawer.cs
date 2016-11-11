﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ASCIIWars.ConsoleGraphics {
    /**
     * <summary>
     * Утилита для рисования меню с выбором. Выбор можно сдвигать
     * вверх с помощью стрелки вверх, и вниз с помощью стрелки вниз.
     * Выбор подтверждается с помощью Enter'а.
     * </summary>
     * 
     * <remarks>
     * <para>Меню выглядит примерно так:
     * <code><![CDATA[
     *  Какую таблетку ты выберешь?     <--- Заголовок, может отсутствовать
     *    никакую --|
     *    красную   |-- Элементы, который можно выбрать
     *  > синюю   --|
     *  ^
     * Стрелка, которая указывает на текущий выбранный елемент
     * ]]></code></para>
     * 
     * <para>Если курсор был сдвинут вверх в самом верхнем положении,
     * или вниз в самом нижнем положении, он буддет поремещён на самую
     * верхнию или нижнию позицию.</para>
     * </remarks>
     */
    public static class MenuDrawer {
        public static void ShowInfoDialog(string info) {
            Select(info, new string[] { "OK" });
        }

        public static void Select(Dictionary<string, Action> actions) {
            string[] choices = actions.Keys.ToArray();
            string choice = Select(choices);
            actions[choice].Invoke();
        }

        public static void Select(string prompt, Dictionary<string, Action> actions) {
            string[] choices = actions.Keys.ToArray();
            string choice = Select(prompt, choices);
            actions[choice].Invoke();
        }

        public static string Select(string prompt, params string[] choices) {
            Console.WriteLine($" {MyConsole.ANSI_ITALIC}{prompt}{MyConsole.ANSI_RESET}");
            string result = Select(choices);
            // Убираем строку с текстом из prompt
            MyConsole.ClearLinesBeforeThis(1);
            MyConsole.MoveCursorUp(1);
            return result;
        }

        public static string Select(params string[] choices) {
            int selectedIndex = 0;
            ConsoleKey pressedKey = ConsoleKey.NoName;

            // Отступаем вниз, чтобы строчка 19 смогла выполниться нормально
            MyConsole.MoveCursorDown(choices.Length);

            while (pressedKey != ConsoleKey.Enter) {
                // 19
                MyConsole.MoveCursorToBeginingOfLine();
                MyConsole.MoveCursorUp(choices.Length);

                for (int choice = 0; choice < choices.Length; choice++) {
                    string choiceTitle = choices[choice];
                    if (selectedIndex == choice)
                        Console.WriteLine($" > {MyConsole.ANSI_BOLD}{choiceTitle}{MyConsole.ANSI_RESET}");
                    else
                        Console.WriteLine($"   {choiceTitle}");
                }

                pressedKey = Console.ReadKey().Key;

                if (pressedKey == ConsoleKey.DownArrow)
                    if (selectedIndex < choices.Length - 1)
                        selectedIndex++;
                    else
                        selectedIndex = 0;
                else if (pressedKey == ConsoleKey.UpArrow)
                    if (selectedIndex > 0)
                        selectedIndex--;
                    else
                        selectedIndex = choices.Length - 1;
            }

            // Перемещаемся выше на 1, потому что, при нажатии Enter каретка сдвигается на 1 линию вниз
            MyConsole.MoveCursorUp(1);
            MyConsole.ClearLinesBeforeThis(choices.Length);
            // Двигаемся как раз к тому месту, где начиналось меню
            MyConsole.MoveCursorUp(choices.Length);

            return choices[selectedIndex];
        }
    }
}
