﻿using Content.Shared.OuterRim.Generator;
using Robust.Client.AutoGenerated;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using FancyWindow = Content.Client.UserInterface.Controls.FancyWindow;

namespace Content.Client.OuterRim;

[GenerateTypedNameReferences]
public sealed partial class GeneratorWindow : FancyWindow
{
    public GeneratorWindow(GeneratorBoundUserInterface bui, EntityUid vis)
    {
        RobustXamlLoader.Load(this);
        IoCManager.InjectDependencies(this);

        EntityView.Sprite = IoCManager.Resolve<IEntityManager>().GetComponent<SpriteComponent>(vis);
        TargetPower.OnValueChanged += (args) =>
        {
            bui.SetTargetPower(args.Value);
        };
    }


    private GeneratorComponentBuiState? _lastState;

    public void Update(GeneratorComponentBuiState state)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (_lastState?.TargetPower != state.TargetPower)
            TargetPower.SetValueWithoutEvent(state.TargetPower);
        Efficiency.Text = SharedGeneratorSystem.CalcFuelEfficiency(state.TargetPower).ToString("P1");
        FuelFraction.Value = state.RemainingFuel - (int) state.RemainingFuel;
        FuelLeft.Text = ((int) MathF.Floor(state.RemainingFuel)).ToString();
        _lastState = state;
    }
}
