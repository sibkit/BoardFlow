using System.Globalization;
using BoardFlow.Formats.Gerber.Entities.Apertures.Macro;
using BoardFlow.Formats.Gerber.Entities.Apertures.Macro.Expressions;
using BoardFlow.Formats.Gerber.Reading.Macro;
using BoardFlow.Formats.Gerber.Reading.Macro.Tokenize;

namespace ConsoleApp;

public static class ExpressionsTest {
    public static void Test(string line) {
        //const string text = "102-(2+2*2)/0.007";
        try {
            Console.Write("Выражение: " + line + "\n");
            var tokens = new Tokenizer().Tokenize(line);
            var builder = new ExpressionBuilder(line);

            var node = builder.Build();
            Console.Write("Синтаксическое дерево: " + PrintExpression(node) + "\n");

            Console.Write("Результат вычисления: ");
            Console.Write(CalculateExpression(node) + "\n");
        } catch (ParseExpressionException pee) {
            Console.Write("Ошибка: "+pee.Message);
            
        } catch (Exception e) {
            Console.Write("Ошибка: ");
            switch (e) {
                case DivideByZeroException:
                    Console.WriteLine("Деление на ноль" + "\n");
                    break;
                default:
                    Console.WriteLine(e.Message + "\n");
                    break;
            }
        }
    }
    
    static string PrintOperation(OperationType operation) {
        return operation switch {
            OperationType.Add => "+",
            OperationType.Subtract => "-",
            OperationType.Multiply => "*",
            OperationType.Divide => "/",
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };
    }

    static string PrintExpression(IExpression? expression) {
        return expression switch {
            OperationExpression op => "(" +PrintExpression(op.LeftExpression) + PrintOperation(op.OperationType) +
                                      PrintExpression(op.RightExpression) + ")",
            ValueExpression ve => ve.Value.ToString(CultureInfo.InvariantCulture),
            ParameterExpression pe => pe.Name,
            null => "null",
            _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, null)
        };
    }



    static double CalculateExpression(IExpression? expression) {
        return expression switch {
            OperationExpression op => op.OperationType switch {
                OperationType.Add => CalculateExpression(op.LeftExpression) + CalculateExpression(op.RightExpression),
                OperationType.Subtract => CalculateExpression(op.LeftExpression) - CalculateExpression(op.RightExpression),
                OperationType.Multiply => CalculateExpression(op.LeftExpression) * CalculateExpression(op.RightExpression),
                OperationType.Divide => CalculateExpression(op.LeftExpression) / CalculateExpression(op.RightExpression),
                _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, null)
            },
            ValueExpression ve => ve.Value,
            ParameterExpression pe => throw new Exception("Not implemented"),
            _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, null)
        };
        ;
    }
    
}