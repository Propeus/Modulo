using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.Abstrato.Util.Console
{
    public static partial class Console
    {
        public static string ReadLine(CancellationToken cancellationToken)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Task.Run(() =>
            {
                try
                {
                    ConsoleKeyInfo keyInfo;
                    var startingLeft = System.Console.CursorLeft;
                    var startingTop = System.Console.CursorTop;
                    var currentIndex = 0;
                    do
                    {
                        var previousLeft = System.Console.CursorLeft;
                        var previousTop = System.Console.CursorTop;
                        while (!System.Console.KeyAvailable)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            System.Threading.Thread.Sleep(50);
                        }
                        keyInfo = System.Console.ReadKey();
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.A:
                            case ConsoleKey.B:
                            case ConsoleKey.C:
                            case ConsoleKey.D:
                            case ConsoleKey.E:
                            case ConsoleKey.F:
                            case ConsoleKey.G:
                            case ConsoleKey.H:
                            case ConsoleKey.I:
                            case ConsoleKey.J:
                            case ConsoleKey.K:
                            case ConsoleKey.L:
                            case ConsoleKey.M:
                            case ConsoleKey.N:
                            case ConsoleKey.O:
                            case ConsoleKey.P:
                            case ConsoleKey.Q:
                            case ConsoleKey.R:
                            case ConsoleKey.S:
                            case ConsoleKey.T:
                            case ConsoleKey.U:
                            case ConsoleKey.V:
                            case ConsoleKey.W:
                            case ConsoleKey.X:
                            case ConsoleKey.Y:
                            case ConsoleKey.Z:
                            case ConsoleKey.Spacebar:
                            case ConsoleKey.Decimal:
                            case ConsoleKey.Add:
                            case ConsoleKey.Subtract:
                            case ConsoleKey.Multiply:
                            case ConsoleKey.Divide:
                            case ConsoleKey.D0:
                            case ConsoleKey.D1:
                            case ConsoleKey.D2:
                            case ConsoleKey.D3:
                            case ConsoleKey.D4:
                            case ConsoleKey.D5:
                            case ConsoleKey.D6:
                            case ConsoleKey.D7:
                            case ConsoleKey.D8:
                            case ConsoleKey.D9:
                            case ConsoleKey.NumPad0:
                            case ConsoleKey.NumPad1:
                            case ConsoleKey.NumPad2:
                            case ConsoleKey.NumPad3:
                            case ConsoleKey.NumPad4:
                            case ConsoleKey.NumPad5:
                            case ConsoleKey.NumPad6:
                            case ConsoleKey.NumPad7:
                            case ConsoleKey.NumPad8:
                            case ConsoleKey.NumPad9:
                            case ConsoleKey.Oem1:
                            case ConsoleKey.Oem102:
                            case ConsoleKey.Oem2:
                            case ConsoleKey.Oem3:
                            case ConsoleKey.Oem4:
                            case ConsoleKey.Oem5:
                            case ConsoleKey.Oem6:
                            case ConsoleKey.Oem7:
                            case ConsoleKey.Oem8:
                            case ConsoleKey.OemComma:
                            case ConsoleKey.OemMinus:
                            case ConsoleKey.OemPeriod:
                            case ConsoleKey.OemPlus:
                                stringBuilder.Insert(currentIndex, keyInfo.KeyChar);
                                currentIndex++;
                                if (currentIndex < stringBuilder.Length)
                                {
                                    var left = System.Console.CursorLeft;
                                    var top = System.Console.CursorTop;
                                    System.Console.Write(stringBuilder.ToString().Substring(currentIndex));
                                    System.Console.SetCursorPosition(left, top);
                                }
                                break;
                            case ConsoleKey.Backspace:
                                if (currentIndex > 0)
                                {
                                    currentIndex--;
                                    stringBuilder.Remove(currentIndex, 1);
                                    var left = System.Console.CursorLeft;
                                    var top = System.Console.CursorTop;
                                    if (left == previousLeft)
                                    {
                                        left = System.Console.BufferWidth - 1;
                                        top--;
                                        System.Console.SetCursorPosition(left, top);
                                    }
                                    System.Console.Write(stringBuilder.ToString().Substring(currentIndex) + " ");
                                    System.Console.SetCursorPosition(left, top);
                                }
                                else
                                {
                                    System.Console.SetCursorPosition(startingLeft, startingTop);
                                }
                                break;
                            case ConsoleKey.Delete:
                                if (stringBuilder.Length > currentIndex)
                                {
                                    stringBuilder.Remove(currentIndex, 1);
                                    System.Console.SetCursorPosition(previousLeft, previousTop);
                                    System.Console.Write(stringBuilder.ToString().Substring(currentIndex) + " ");
                                    System.Console.SetCursorPosition(previousLeft, previousTop);
                                }
                                else
                                    System.Console.SetCursorPosition(previousLeft, previousTop);
                                break;
                            case ConsoleKey.LeftArrow:
                                if (currentIndex > 0)
                                {
                                    currentIndex--;
                                    var left = System.Console.CursorLeft - 2;
                                    var top = System.Console.CursorTop;
                                    if (left < 0)
                                    {
                                        left = System.Console.BufferWidth + left;
                                        top--;
                                    }
                                    System.Console.SetCursorPosition(left, top);
                                    if (currentIndex < stringBuilder.Length - 1)
                                    {
                                        System.Console.Write(stringBuilder[currentIndex].ToString() + stringBuilder[currentIndex + 1]);
                                        System.Console.SetCursorPosition(left, top);
                                    }
                                }
                                else
                                {
                                    System.Console.SetCursorPosition(startingLeft, startingTop);
                                    if (stringBuilder.Length > 0)
                                        System.Console.Write(stringBuilder[0]);
                                    System.Console.SetCursorPosition(startingLeft, startingTop);
                                }
                                break;
                            case ConsoleKey.RightArrow:
                                if (currentIndex < stringBuilder.Length)
                                {
                                    System.Console.SetCursorPosition(previousLeft, previousTop);
                                    System.Console.Write(stringBuilder[currentIndex]);
                                    currentIndex++;
                                }
                                else
                                {
                                    System.Console.SetCursorPosition(previousLeft, previousTop);
                                }
                                break;
                            case ConsoleKey.Home:
                                if (stringBuilder.Length > 0 && currentIndex != stringBuilder.Length)
                                {
                                    System.Console.SetCursorPosition(previousLeft, previousTop);
                                    System.Console.Write(stringBuilder[currentIndex]);
                                }
                                System.Console.SetCursorPosition(startingLeft, startingTop);
                                currentIndex = 0;
                                break;
                            case ConsoleKey.End:
                                if (currentIndex < stringBuilder.Length)
                                {
                                    System.Console.SetCursorPosition(previousLeft, previousTop);
                                    System.Console.Write(stringBuilder[currentIndex]);
                                    var left = previousLeft + stringBuilder.Length - currentIndex;
                                    var top = previousTop;
                                    while (left > System.Console.BufferWidth)
                                    {
                                        left -= System.Console.BufferWidth;
                                        top++;
                                    }
                                    currentIndex = stringBuilder.Length;
                                    System.Console.SetCursorPosition(left, top);
                                }
                                else
                                    System.Console.SetCursorPosition(previousLeft, previousTop);
                                break;
                            default:
                                System.Console.SetCursorPosition(previousLeft, previousTop);
                                break;
                        }
                    } while (keyInfo.Key != ConsoleKey.Enter);
                    System.Console.WriteLine();
                }
                catch
                {
                    //MARK: Change this based on your need. See description below.
                    stringBuilder.Clear();
                }
            }).Wait();
            return stringBuilder.ToString();
        }
    }
}
