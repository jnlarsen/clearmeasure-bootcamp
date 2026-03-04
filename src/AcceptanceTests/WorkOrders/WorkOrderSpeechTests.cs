using System.Text.RegularExpressions;
using ClearMeasure.Bootcamp.UI.Shared.Pages;

namespace ClearMeasure.Bootcamp.AcceptanceTests.WorkOrders;

public class WorkOrderSpeechTests : AcceptanceTestBase
{
    [Test, Retry(2)]
    public async Task ShouldRenderSpeakTitleButtonOnWorkOrderManagePage()
    {
        await LoginAsCurrentUser();

        var order = await CreateAndSaveNewWorkOrder();
        order = await ClickWorkOrderNumberFromSearchPage(order);

        var speakTitleButton = Page.GetByTestId(nameof(WorkOrderManage.Elements.SpeakTitle));
        await Expect(speakTitleButton).ToBeVisibleAsync();
    }

    [Test, Retry(2)]
    public async Task ShouldRenderSpeakDescriptionButtonOnWorkOrderManagePage()
    {
        await LoginAsCurrentUser();

        var order = await CreateAndSaveNewWorkOrder();
        order = await ClickWorkOrderNumberFromSearchPage(order);

        var speakDescriptionButton = Page.GetByTestId(nameof(WorkOrderManage.Elements.SpeakDescription));
        await Expect(speakDescriptionButton).ToBeVisibleAsync();
    }

    [Test, Retry(2)]
    public async Task ShouldClickSpeakTitleButtonWithoutError()
    {
        await LoginAsCurrentUser();

        var order = await CreateAndSaveNewWorkOrder();
        order = await ClickWorkOrderNumberFromSearchPage(order);

        await Input(nameof(WorkOrderManage.Elements.Title), "Test speech title");
        await Click(nameof(WorkOrderManage.Elements.SpeakTitle));

        await Expect(Page).ToHaveURLAsync(new Regex("/workorder/manage/"));
    }

    [Test, Retry(2)]
    public async Task ShouldClickSpeakDescriptionButtonWithoutError()
    {
        await LoginAsCurrentUser();

        var order = await CreateAndSaveNewWorkOrder();
        order = await ClickWorkOrderNumberFromSearchPage(order);

        await Input(nameof(WorkOrderManage.Elements.Description), "Test speech description");
        await Click(nameof(WorkOrderManage.Elements.SpeakDescription));

        await Expect(Page).ToHaveURLAsync(new Regex("/workorder/manage/"));
    }

    [Test, Retry(2)]
    public async Task ShouldShowSpeakButtonsOnReadOnlyWorkOrder()
    {
        await LoginAsCurrentUser();

        var order = await CreateAndSaveNewWorkOrder();
        order = await ClickWorkOrderNumberFromSearchPage(order);

        order = await AssignExistingWorkOrder(order, CurrentUser.UserName);
        order = await ClickWorkOrderNumberFromSearchPage(order);

        order = await BeginExistingWorkOrder(order);
        order = await ClickWorkOrderNumberFromSearchPage(order);

        order = await CompleteExistingWorkOrder(order);

        // The creator now has a Reassign command on completed work orders,
        // so switch to a non-creator user to get a read-only view.
        var viewer = CreateTestUser(TestTag);
        CurrentUser = viewer;
        await Page.GotoAsync("/login");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Select(nameof(Login.Elements.User), viewer.UserName);
        await Click(nameof(Login.Elements.LoginButton));
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Page.GotoAsync($"/workorder/manage/{order.Number}");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page.GetByTestId(nameof(WorkOrderManage.Elements.ReadOnlyMessage))).ToBeVisibleAsync();
        await Expect(Page.GetByTestId(nameof(WorkOrderManage.Elements.SpeakTitle))).ToBeVisibleAsync();
        await Expect(Page.GetByTestId(nameof(WorkOrderManage.Elements.SpeakDescription))).ToBeVisibleAsync();
    }
}
