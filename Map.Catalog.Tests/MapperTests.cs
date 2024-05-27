using System.Text.Json.Nodes;
using Map.Catalog.Api.Logic;

namespace Map.Catalog.Tests;

public class MapperTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetValueTest_Return_1()
    {
        var mapper = new Mapper();
        var joSource = new JsonObject
        {
            { "a", 1 }
        };
        var actual = mapper.GetValue(joSource, "a");
        var expected = 1;
        Assert.That(actual.GetValue<int>(), Is.EqualTo(expected));
    }
    [Test]
    public void GetValueTest_Return_2()
    {
        var mapper = new Mapper();
        var joSource = new JsonObject
        {
            { "a", new JsonObject {
                { "b", 2 }
            }
            }
        };
        var actual = mapper.GetValue(joSource, "a.b");
        var expected = 2;
        Assert.That(actual.GetValue<int>(), Is.EqualTo(expected));
    }
    [Test]
    public void GetValueTest_Return_3()
    {
        var mapper = new Mapper();
        var joSource = new JsonObject
        {
            { "a", new JsonObject {
                { "b", new JsonObject{
                    { "c", 3 }
                }
                }
            }
            }
        };
        var actual = mapper.GetValue(joSource, "a.b.c");
        var expected = 3;
        Assert.That(actual.GetValue<int>(), Is.EqualTo(expected));
    }

    [Test]
    public void GetValueTest_Return_NULL()
    {
        var mapper = new Mapper();
        var joSource = new JsonObject
        {
            { "a", new JsonObject {
                { "b", new JsonObject{
                    { "c", null }
                }
                }
            }
            }
        };
        Assert.IsNull(mapper.GetValue(joSource, "a.b.c"));
        Assert.IsNull(mapper.GetValue(joSource, "a.b.c.d"));
    }

    [Test]
    public void ConvertTest_a_3()
    {
        var joSource = new JsonObject
        {
            { "a", new JsonObject {
                { "b", new JsonObject{
                    { "c", 3 }
                }
                }
            }
            }
        };
        var sourcePath = "a.b.c";
        var destinationPath = "a";
        var mapper = new Mapper();
        var destination = mapper.Convert(joSource, [(sourcePath, destinationPath)]);
        var actual = destination["a"]!.GetValue<int>();
        var expected = 3;
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void ConvertTest_b_1()
    {
        var joSource = new JsonObject
        {
            { "a", new JsonObject {
                { "b", new JsonObject{
                    { "c", 1 }
                }
                }
            }
            }
        };
        var sourcePath = "a.b.c";
        var destinationPath = "a.b";
        var mapper = new Mapper();
        var destination = mapper.Convert(joSource, [(sourcePath, destinationPath)]);
        var actual = destination["a"]!["b"]!.GetValue<int>();
        var expected = 1;
        Assert.That(actual, Is.EqualTo(expected));
    }
    [Test]
    public void ConvertTest_b_2_c_3()
    {
        var joSource = new JsonObject
        {
            { "b", 2},
            { "c", 3}
        };
        var sourcePathB = "b";
        var destinationPathB = "a.b";
        var sourcePathC = "c";
        var destinationPathC = "a.c";
        var mapper = new Mapper();
        var destination = mapper.Convert(joSource,
        [
            (sourcePathB, destinationPathB),
            (sourcePathC, destinationPathC)
            ]
        );
        var actualB = destination["a"]!["b"]!.GetValue<int>();
        var expectedB = 2;
        Assert.That(actualB, Is.EqualTo(expectedB));

        var actualC = destination["a"]!["c"]!.GetValue<int>();
        var expectedC = 3;
        Assert.That(actualC, Is.EqualTo(expectedC));
    }
}