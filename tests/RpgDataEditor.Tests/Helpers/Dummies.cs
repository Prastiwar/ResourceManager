using RpgDataEditor.Models;
using System.Collections.Generic;

namespace RpgDataEditor.Tests
{
    public class Dummies
    {
        public static Dialogue Dialogue => new Dialogue() {
            Id = int.MaxValue / 2,
            StartQuestId = 1,
            Title = "TestTitle",
            Category = "TestCategory",
            Message = "TestMessage",
            Requirements = new List<Requirement>(),
            Options = new List<DialogueOption>()
        };

        public static Quest Quest => new Quest() {
            Id = int.MaxValue / 2,
            Title = "TestTitle",
            Category = "TestCategory",
            Message = "TestMessage",
            CompletionTask = new ReachQuestTask(),
            Requirements = new List<Requirement>(),
        };

        public static Npc Npc => new Npc() {
            Id = int.MaxValue / 2,
            Name = "TestName",
            Job = new TraderNpcJob() {
                Items = new List<TradeItem>() {
                    new TradeItem() {
                        ItemId = "id",
                        Buy = 10,
                    }
                }
            },
            Position = new Position(),
            TalkData = new TalkData(),
            Attributes = new List<AttributeData>(),
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
