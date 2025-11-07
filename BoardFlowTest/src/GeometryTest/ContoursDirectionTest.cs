using BoardFlow.Formats.Bfg.Entities;
using BoardFlow.Formats.Bfg.Entities.GraphicElements;
using BoardFlow.Formats.Bfg.Handling;

namespace BoardFlowTest.GeometryTest;

public class ContoursDirectionTest {
    [Fact]
    public void TestContoursDirection() {
        var p1 = new Painter<Contour>(10, 10);
        p1.LineToInc(20,20);
        p1.LineToInc(20,0);
        p1.LineToInc(0,-40);
        p1.LineToInc(-20,0);
        p1.LineToInc(-20,20);
        var c1 = p1.Root;
        Assert.Equal(RotationDirection.Clockwise, Contours.GetRotationDirection(c1));
        var c2 = Contours.GetReversed(c1);
        Assert.Equal(RotationDirection.CounterClockwise, Contours.GetRotationDirection(c2));
    }
}