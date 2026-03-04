using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.LlmGateway;
using Microsoft.Extensions.AI;

namespace ClearMeasure.Bootcamp.UI.Server;

/// <summary>
///     AI agent responsible for reformatting work order title and description fields
/// </summary>
public class WorkOrderReformatAgent(
    ChatClientFactory chatClientFactory,
    ILogger<WorkOrderReformatAgent> logger)
{
    /// <summary>
    ///     Reformats a work order's title using proper title-case,
    ///     corrects the description for grammar and punctuation,
    ///     and translates the description into the assignee's preferred language when an assignee is set.
    ///     Returns the updated title and description, or null if no changes are needed.
    /// </summary>
    public async Task<ReformatResult?> ReformatWorkOrderAsync(WorkOrder workOrder)
    {
        try
        {
            var chatClient = await chatClientFactory.GetChatClient();

            var preferredLanguage = workOrder.Assignee?.PreferredLanguage;

            var translationInstruction = preferredLanguage != null
                ? $"Correct the description for grammar and punctuation, then translate it into {preferredLanguage}. Do not change the meaning."
                : "Correct the description for grammar and punctuation. Do not change the meaning.";

            var systemPrompt = $"""
                                You are an AI agent responsible for reformatting work order fields.
                                You will receive a work order title and description.

                                Your tasks:
                                1. {translationInstruction}
                                2. Format the title using proper title-case (capitalize the first letter of each significant word). Do not change the meaning.

                                If no changes are needed, respond with exactly: NO_CHANGES

                                Otherwise respond in this exact format (two lines only):
                                TITLE: <corrected title>
                                DESCRIPTION: <corrected description>
                                """;

            var workOrderInfo = preferredLanguage != null
                ? $"""
                    Title: {workOrder.Title}
                    Description: {workOrder.Description}
                    Preferred Language: {preferredLanguage}
                    """
                : $"""
                    Title: {workOrder.Title}
                    Description: {workOrder.Description}
                    """;

            var messages = new List<ChatMessage>
            {
                new(ChatRole.System, systemPrompt),
                new(ChatRole.User, workOrderInfo)
            };

            var response = await chatClient.GetResponseAsync(messages);
            var responseText = response.Text?.Trim();

            if (string.IsNullOrWhiteSpace(responseText) ||
                responseText.Equals("NO_CHANGES", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("No reformatting needed for WorkOrder {WorkOrderNumber}",
                    workOrder.Number);
                return null;
            }

            var result = ParseResponse(responseText, workOrder);

            if (result != null)
            {
                logger.LogInformation(
                    "Reformatted WorkOrder {WorkOrderNumber}: Title changed={TitleChanged}, Description changed={DescriptionChanged}",
                    workOrder.Number,
                    result.Title != workOrder.Title,
                    result.Description != workOrder.Description);
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reformatting WorkOrder {WorkOrderNumber}",
                workOrder.Number);
            return null;
        }
    }

    internal static ReformatResult? ParseResponse(string responseText, WorkOrder workOrder)
    {
        var lines = responseText.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        string? title = null;
        string? description = null;

        foreach (var line in lines)
        {
            if (line.StartsWith("TITLE:", StringComparison.OrdinalIgnoreCase))
            {
                title = line["TITLE:".Length..].Trim();
            }
            else if (line.StartsWith("DESCRIPTION:", StringComparison.OrdinalIgnoreCase))
            {
                description = line["DESCRIPTION:".Length..].Trim();
            }
        }

        title ??= workOrder.Title;
        description ??= workOrder.Description;

        if (title == workOrder.Title && description == workOrder.Description)
        {
            return null;
        }

        return new ReformatResult(title!, description!);
    }
}

public record ReformatResult(string Title, string Description);
