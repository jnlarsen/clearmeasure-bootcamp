using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.StateCommands;
using ClearMeasure.Bootcamp.Core.Services;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model.StateCommands;

[TestFixture]
public class CompleteToAssignedCommandTests : StateCommandBaseTests
{
    [Test]
    public void IsValid_CreatorWithCompleteStatus_ReturnsTrue()
    {
        var order = new WorkOrder();
        order.Status = WorkOrderStatus.Complete;
        var employee = new Employee();
        order.Creator = employee;

        var command = new CompleteToAssignedCommand(order, employee);
        Assert.That(command.IsValid(), Is.True);
    }

    [Test]
    public void IsValid_NonCreatorWithCompleteStatus_ReturnsFalse()
    {
        var order = new WorkOrder();
        order.Status = WorkOrderStatus.Complete;
        var creator = new Employee();
        var differentEmployee = new Employee();
        order.Creator = creator;

        var command = new CompleteToAssignedCommand(order, differentEmployee);
        Assert.That(command.IsValid(), Is.False);
    }

    [Test]
    public void IsValid_CreatorWithNonCompleteStatus_ReturnsFalse()
    {
        var order = new WorkOrder();
        order.Status = WorkOrderStatus.Draft;
        var employee = new Employee();
        order.Creator = employee;

        var command = new CompleteToAssignedCommand(order, employee);
        Assert.That(command.IsValid(), Is.False);
    }

    [Test]
    public void Execute_SetsAssignedDateToCurrentDateTime()
    {
        var order = new WorkOrder();
        order.Number = "123";
        order.Status = WorkOrderStatus.Complete;
        var employee = new Employee();
        order.Creator = employee;
        var context = new StateCommandContext();

        var command = new CompleteToAssignedCommand(order, employee);
        command.Execute(context);

        Assert.That(order.AssignedDate, Is.EqualTo(context.CurrentDateTime));
    }

    [Test]
    public void Execute_ClearsCompletedDate()
    {
        var order = new WorkOrder();
        order.Number = "123";
        order.Status = WorkOrderStatus.Complete;
        order.CompletedDate = DateTime.Now;
        var employee = new Employee();
        order.Creator = employee;

        var command = new CompleteToAssignedCommand(order, employee);
        command.Execute(new StateCommandContext());

        Assert.That(order.CompletedDate, Is.Null);
    }

    [Test]
    public void Execute_ChangesStatusToAssigned()
    {
        var order = new WorkOrder();
        order.Number = "123";
        order.Status = WorkOrderStatus.Complete;
        var employee = new Employee();
        order.Creator = employee;

        var command = new CompleteToAssignedCommand(order, employee);
        command.Execute(new StateCommandContext());

        Assert.That(order.Status, Is.EqualTo(WorkOrderStatus.Assigned));
    }

    protected override StateCommandBase GetStateCommand(WorkOrder order, Employee employee)
    {
        return new CompleteToAssignedCommand(order, employee);
    }
}
