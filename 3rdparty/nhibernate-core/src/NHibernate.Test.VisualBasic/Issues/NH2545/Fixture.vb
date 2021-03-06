Imports NUnit.Framework
Imports NHibernate.Test.NHSpecificTest
Imports NHibernate.Linq

Namespace Issues

    Namespace NH2545

        <TestFixture()> _
        Public Class Fixture
            Inherits IssueTestCase

            Protected Overrides Sub OnSetUp()

                Using session As ISession = OpenSession()

                    Dim e1 = New Entity
                    e1.Name = "Bob"
                    session.Save(e1)

                    Dim e2 = New Entity
                    e2.Name = "Sally"
                    session.Save(e2)

                    session.Flush()

                End Using

            End Sub

            Protected Overrides Sub OnTearDown()

                Using session As ISession = OpenSession()

                    session.Delete("from System.Object")
                    session.Flush()

                End Using

            End Sub

            <Test()> _
            Public Sub LinqStringEquality()

                Using session As ISession = OpenSession()

                    Dim result = (From e In session.Query(Of Entity)() _
                                 Where e.Name = "Bob" _
                                 Select e).ToList()

                    Assert.AreEqual(1, result.Count)

                End Using

            End Sub

        End Class

    End Namespace

End Namespace
