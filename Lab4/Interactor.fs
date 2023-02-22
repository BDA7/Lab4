module Interactor
open FunctionsUI
open Xamarin.Forms

let r = System.Random()

let generateRandomNum () =
    let choosingValues = [| "2"; "4" |]
    let value = choosingValues.[(r.Next(0, 2))]
    (value)


let rec shifLeftUp (arr: string[]) (value: string) (i: int) =
    if (i < 0) || (arr.[i] <> "") then
        (arr)
    else
        arr.[i + 1] <- ""
        arr.[i] <- value
        shifLeftUp arr value (i - 1)


let rec shiftRightDown (arr: string[]) (value: string) (i: int) (numberRows: int) =
    if (i > numberRows - 1) || (arr.[i] <> "") then
        (arr)
    else
        arr.[i] <- value
        arr.[i - 1] <- ""
        shiftRightDown arr value (i + 1) numberRows


let myHandlerLeft (numberRows: int) (positions: string[,]) (frames: Frame[,]) =
    let shift () =
        for i in 0 .. (numberRows - 1) do
            for j in 0 .. (numberRows - 1) do
                if positions.[i, j] <> "" then
                    positions.[i, *] <- shifLeftUp positions.[i, *] positions.[i, j] (j - 1)

        ()

    shift ()

    for i in 0 .. (numberRows - 1) do
        for j in 0 .. (numberRows - 2) do
            if (positions.[i, j] = positions.[i, j + 1]) && (positions.[i, j] <> "") then
                let value = positions.[i, j] |> int
                positions.[i, j] <- (value * 2).ToString()
                positions.[i, j + 1] <- ""

    shift ()
    updateGrid positions numberRows frames


let handlerRight (numberRows: int) (positions: string[,]) (frames: Frame[,]) =
    let shift () =
        for i in 0 .. (numberRows - 1) do
            for j in (numberRows - 1) .. -1 .. 0 do
                if (positions.[i, j] <> "") then
                    positions.[i, *] <- shiftRightDown positions.[i, *] positions.[i, j] (j + 1) numberRows

    shift ()

    for i in 0 .. (numberRows - 1) do
        for j in (numberRows - 1) .. -1 .. 1 do
            if (positions.[i, j] = positions.[i, j - 1]) && (positions.[i, j] <> "") then
                let value = positions.[i, j] |> int
                positions.[i, j] <- (value * 2).ToString()
                positions.[i, j - 1] <- ""

    shift ()

    updateGrid positions numberRows frames


let handlerUp (numberRows: int) (positions: string[,]) (frames: Frame[,]) =
    let shift () =
        for i in 0 .. (numberRows - 1) do
            for j in 0 .. (numberRows - 1) do
                if (positions.[j, i] <> "") then
                    positions.[*, i] <- shifLeftUp positions.[*, i] positions.[j, i] (j - 1)

    shift ()

    for i in 0 .. (numberRows - 1) do
        for j in 0 .. (numberRows - 2) do
            if (positions.[j, i] = positions.[j + 1, i]) && (positions.[j, i] <> "") then
                let value = positions.[j, i] |> int
                positions.[j, i] <- (value * 2).ToString()
                positions.[j + 1, i] <- ""

    shift ()

    updateGrid positions numberRows frames


let handlerDown (numberRows: int) (positions: string[,]) (frames: Frame[,]) =
    let shift () =
        for i in 0 .. (numberRows - 1) do
            for j in (numberRows - 1) .. -1 .. 0 do
                if (positions.[j, i] <> "") then
                    positions.[*, i] <- shiftRightDown positions.[*, i] positions.[j, i] (j + 1) numberRows

    shift ()

    for i in 0 .. (numberRows - 1) do
        for j in (numberRows - 1) .. -1 .. 1 do
            if (positions.[j, i] = positions.[j - 1, i]) && (positions.[j, i] <> "") then
                let value = positions.[j, i] |> int
                positions.[j, i] <- (value * 2).ToString()
                positions.[j - 1, i] <- ""

    shift ()

    updateGrid positions numberRows frames