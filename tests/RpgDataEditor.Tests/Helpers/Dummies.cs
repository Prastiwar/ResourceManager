using RpgDataEditor.Models;
using System.Collections.Generic;

namespace RpgDataEditor.Tests
{
    public static class Dummies
    {
        private static readonly List<Requirement> requirements = new List<Requirement>() {
                new MoneyRequirement() { Money = 10 },
                new DialogueRequirement() { DialogueId = 1 },
                new ItemRequirement() { ItemId = 1 },
                new QuestRequirement() {
                    QuestId = 1,
                    Stage = QuestStage.Done
                }
            };

        public static Dialogue Dialogue => new Dialogue() {
            Id = int.MaxValue / 2,
            StartQuestId = 1,
            Title = "TestTitle",
            Category = "TestCategory",
            Message = "TestMessage",
            Requirements = requirements,
            Options = new List<DialogueOption>() {
                new DialogueOption() {
                    Message = "TestOptionMessage",
                    Requirements = requirements,
                }
            }
        };

        public static Quest Quest => new Quest() {
            Id = int.MaxValue / 2,
            Title = "TestTitle",
            Category = "TestCategory",
            Message = "TestMessage",
            CompletionTask = new ReachQuestTask() {
                Distance = 5,
                Pos = new Position() { Y = 10 }
            },
            Requirements = requirements,
            Tasks = new List<QuestTask>() {
                new ReachQuestTask() {
                    Distance = 5,
                    Pos = new Position() { Z = 10 }
                },
                new KillQuestTask() {
                    Amount = 1,
                    TargetId = 1
                },
                new EntityInteractQuestTask() { EntityId = 1 },
                new ItemInteractQuestTask() { ItemId = 1 }
            }
        };

        public static Npc Npc => new Npc() {
            Id = int.MaxValue / 2,
            Name = "TestName",
            Job = new TraderNpcJob() {
                Items = new List<TradeItem>() {
                    new TradeItem() {
                        ItemId = 1,
                        Buy = 10,
                    }
                }
            },
            Position = new Position() { X = 10 },
            TalkData = new TalkData() {
                TalkRange = 5,
                InitationDialogues = new List<int>() { int.MaxValue / 2 }
            },
            Attributes = new List<AttributeData>() {
                new AttributeData("Health", 100)
            },
        };

        public static Dialogue UpdateDialogue {
            get {
                Dialogue dialogue = Dialogue;
                dialogue.Id = int.MaxValue / 3;
                return dialogue;
            }
        }

        public static Quest UpdateQuest {
            get {
                Quest quest = Quest;
                quest.Id = int.MaxValue / 3;
                return quest;
            }
        }

        public static Npc UpdateNpc {
            get {
                Npc npc = Npc;
                npc.Id = int.MaxValue / 3;
                return npc;
            }
        }

        public static Dialogue DeleteDialogue {
            get {
                Dialogue dialogue = Dialogue;
                dialogue.Id = int.MaxValue / 4;
                return dialogue;
            }
        }

        public static Quest DeleteQuest {
            get {
                Quest quest = Quest;
                quest.Id = int.MaxValue / 4;
                return quest;
            }
        }

        public static Npc DeleteNpc {
            get {
                Npc npc = Npc;
                npc.Id = int.MaxValue / 4;
                return npc;
            }
        }

        public static Dialogue CategoryDialogue {
            get {
                Dialogue dialogue = Dialogue;
                dialogue.Id = int.MaxValue / 5;
                return dialogue;
            }
        }

        public static Quest CategoryQuest {
            get {
                Quest quest = Quest;
                quest.Id = int.MaxValue / 5;
                return quest;
            }
        }
    }
}
