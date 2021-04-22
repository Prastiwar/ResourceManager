//    public class DefaultLocationToSimpleResourceConverter : ILocationToSimpleResourceConverter
//    {
//        public PresentableData CreateSimpleData(string location)
//        {
//            int resource = GetResource(location);
//            if (resource == 0 || resource == 1)
//            {
//                PresentableData data = CreateSimpleData(resource, location);
//                int lastDimIndex = location.LastIndexOf('/');
//                int categoryStartIndex = location.LastIndexOf('/', lastDimIndex - 1) + 1;
//                string category = location[categoryStartIndex..lastDimIndex];
//                return new PresentableCategoryData(data.RealType) {
//                    Id = data.Id,
//                    Name = data.Name,
//                    Category = category
//                };
//            }
//            else if (resource == 2)
//            {
//                return CreateSimpleData(resource, location);
//            }
//            throw new NotSupportedException("Creating simple data from unknown location is not supported");
//        }

//        private PresentableData CreateSimpleData(int resource, string location)
//        {
//            string fileName = Path.GetFileNameWithoutExtension(location);
//            int id = -1;
//            string name = fileName;
//            int index = fileName.IndexOf('_');
//            if (index >= 0)
//            {
//                string idString = fileName.Substring(0, index);
//                if (int.TryParse(idString, out int newId))
//                {
//                    id = newId;
//                }
//                name = fileName[(index + 1)..];
//            }
//            return new PresentableData(typeConverter.GetResourceType(resource)) {
//                Id = id,
//                Name = name
//            };
//        }

//        private int GetResource(string location)
//        {
//            string questRoot = pathConverter.ToRelativeRoot((int)RPGResource.Quest);
//            string dialogueRoot = pathConverter.ToRelativeRoot((int)RPGResource.Dialogue);
//            string npcRoot = pathConverter.ToRelativeRoot((int)RPGResource.Npc);
//            if (location.Contains(questRoot))
//            {
//                return (int)RPGResource.Quest;
//            }
//            else if (location.Contains(dialogueRoot))
//            {
//                return (int)RPGResource.Dialogue;
//            }
//            else if (location.Contains(npcRoot))
//            {
//                return (int)RPGResource.Npc;
//            }
//            return -1;
//        }