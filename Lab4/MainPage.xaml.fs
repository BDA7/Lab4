namespace Lab4

open System
open Xamarin.Forms
open Xamarin.Forms.Xaml
open FunctionsUI
open Interactor

type MainPage(numberRows: int) as this =
    inherit ContentPage()
    let r = System.Random()
    let frames = Array2D.zeroCreate<Frame> numberRows numberRows
    let positions = Array2D.create (numberRows) (numberRows) ""


    let checkEndGame () =
        let oldValue = positions
        updateGrid positions numberRows frames

        if (oldValue = positions) then
            this.Navigation.PopAsync() |> ignore

        ()


    let generateNewValue () =
        let freeBoxes =
            positions
            |> Array2D.mapi (fun i j v ->
                match v with
                | "" -> (i, j)
                | _ -> (-1, -1))

        let free: (int * int) list =
            freeBoxes
            |> Seq.cast<int * int>
            |> Seq.toList
            |> List.filter (fun els -> els <> (-1, -1))

        if (free.Length > 0) then
            let idx = r.Next(0, free.Length - 1)
            let newValue = generateRandomNum ()
            let i, j = free.[idx]
            positions.[i, j] <- newValue
            updateGrid positions numberRows frames
        else
            checkEndGame ()


    let rendomizeSetup () =
        generateNewValue ()
        generateNewValue ()



    let addRecogrinzers (grid: Grid) =
        let leftRecognizer = SwipeGestureRecognizer(Direction = SwipeDirection.Left)

        leftRecognizer.Swiped.Add(fun _ ->
            myHandlerLeft numberRows positions frames
            generateNewValue ())

        let rightRecognizer = SwipeGestureRecognizer(Direction = SwipeDirection.Right)

        rightRecognizer.Swiped.Add(fun _ ->
            handlerRight numberRows positions frames
            generateNewValue ())

        let upRecognizer = SwipeGestureRecognizer(Direction = SwipeDirection.Up)

        upRecognizer.Swiped.Add(fun _ ->
            handlerUp numberRows positions frames
            generateNewValue ())

        let downRecognizer = SwipeGestureRecognizer(Direction = SwipeDirection.Down)

        downRecognizer.Swiped.Add(fun _ ->
            handlerDown numberRows positions frames
            generateNewValue ())

        grid.GestureRecognizers.Add leftRecognizer
        grid.GestureRecognizers.Add rightRecognizer
        grid.GestureRecognizers.Add upRecognizer
        grid.GestureRecognizers.Add downRecognizer



    let setupRootView () =
        let rootView = RelativeLayout()
        rootView.BackgroundColor <- Color.Beige
        let grid = setupGrid numberRows frames
        rendomizeSetup ()
        addRecogrinzers grid

        rootView.Children.Add(
            grid,
            Constraint.Constant(10.0),
            Constraint.Constant(0.0),
            Constraint.RelativeToParent(fun x -> x.Width - 20.0),
            Constraint.RelativeToParent(fun x -> x.Width - 20.0)
        )

        (rootView)


    let baseViewLoad = base.Content <- setupRootView ()
