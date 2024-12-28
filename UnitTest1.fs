module UnitTest1

open Xunit
open FsUnit.Xunit

[<Fact>]
let Test1 () = Assert.True(true)

[<Fact>]
let Test2 () = true |> should equal true
