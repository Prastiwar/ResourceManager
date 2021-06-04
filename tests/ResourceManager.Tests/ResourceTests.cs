using ResourceManager.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace ResourceManager.Tests
{
    public class ResourceTests
    {
        public class DummyModel
        {
            public float Floating { get; set; }
        }

        public class CustomResource : Resource
        {
            public CustomResource() => Add<string>("Text", "Foo");
        }

        public class DummyComplexModel
        {
            public int Id { get; set; }

            public DummyModel Model { get; set; }

            public IList<int> SimpleList { get; set; }

            public int[] SimpleArray { get; set; }

            public IList<DummyModel> Models { get; set; }

            public DummyModel[] ModelsArray { get; set; }

            public CustomResource Resource { get; set; }
        }

        [Fact]
        public void ResoureEquality()
        {
            Resource resource1 = new Resource(typeof(DummyModel));
            Resource resource2 = new Resource(typeof(DummyModel));

            Assert.True(resource1 == resource2);
            Assert.True(resource1.Equals(resource2));
            Assert.True(EqualityComparer<Resource>.Default.Equals(resource1, resource2));
        }
        [Fact]
        public void CreateResoureFromComplexModelAlternative()
        {
            DummyComplexModel complexModel = new DummyComplexModel() {
                Id = 123,
                Model = new DummyModel(),
                SimpleList = new List<int>() { 1, 2, 3 },
                SimpleArray = new int[] { 1, 2 },
                Models = new List<DummyModel>() { new DummyModel(), new DummyModel() { Floating = 10 } }
            };
            complexModel.ModelsArray = complexModel.Models.ToArray();

            Resource resource = new Resource(typeof(DummyComplexModel));
            resource.UpdateProperties(complexModel);
            KeyValuePair<string, ResourceProperty>[] properties = resource.GetProperties().ToArray();
            Assert.Equal(5, properties.Length);
            Assert.Equal(nameof(DummyComplexModel.Id), properties[0].Key);
            Assert.Equal(nameof(DummyComplexModel.Model), properties[1].Key);
            Assert.Equal(nameof(DummyComplexModel.SimpleList), properties[2].Key);
            Assert.Equal(nameof(DummyComplexModel.SimpleArray), properties[3].Key);
            Assert.Equal(nameof(DummyComplexModel.Models), properties[4].Key);
            Assert.Equal(nameof(DummyComplexModel.ModelsArray), properties[5].Key);
            Assert.Equal(nameof(DummyComplexModel.Resource), properties[6].Key);

            Assert.Equal(typeof(int), properties[0].Value.Type);
            Assert.Equal(typeof(DummyModel), properties[1].Value.Type);
            Assert.Equal(typeof(List<int>), properties[2].Value.Type);
            Assert.Equal(typeof(int[]), properties[3].Value.Type);
            Assert.Equal(typeof(List<DummyModel>), properties[4].Value.Type);
            Assert.Equal(typeof(DummyModel[]), properties[5].Value.Type);
            Assert.Equal(typeof(CustomResource), properties[6].Value.Type);

            Assert.Equal(typeof(int), properties[0].Value.DeclaredType);
            Assert.Equal(typeof(DummyModel), properties[1].Value.DeclaredType);
            Assert.Equal(typeof(IList<int>), properties[2].Value.DeclaredType);
            Assert.Equal(typeof(int[]), properties[3].Value.DeclaredType);
            Assert.Equal(typeof(IList<DummyModel>), properties[4].Value.DeclaredType);
            Assert.Equal(typeof(DummyModel[]), properties[5].Value.DeclaredType);
            Assert.Equal(typeof(CustomResource), properties[6].Value.DeclaredType);

            Assert.Equal(complexModel.Id, properties[0].Value.Value);
            Assert.Equal(complexModel.Model, properties[1].Value.Value);

            IList<int> simpleList = properties[2].Value.Value as IList<int>;
            Assert.NotNull(simpleList);
            Assert.Equal(complexModel.SimpleList.Count, simpleList.Count);
            Assert.True(complexModel.SimpleList.SequenceEqual(simpleList));
            Action<int>[] intInspectors = new Action<int>[simpleList.Count];
            for (int i = 0; i < simpleList.Count; i++)
            {
                int index = i;
                intInspectors[i] = resource => {
                    Assert.Equal(complexModel.SimpleList[index], resource);
                };
            }
            Assert.Collection(simpleList, intInspectors);

            int[] simpleArray = properties[3].Value.Value as int[];
            Assert.NotNull(simpleArray);
            Assert.Equal(complexModel.SimpleArray.Length, simpleArray.Length);
            Assert.True(complexModel.SimpleArray.SequenceEqual(simpleArray));
            intInspectors = new Action<int>[simpleArray.Length];
            for (int i = 0; i < simpleArray.Length; i++)
            {
                int index = i;
                intInspectors[i] = resource => {
                    Assert.Equal(complexModel.SimpleArray[index], resource);
                };
            }
            Assert.Collection(simpleArray, intInspectors);

            IList<DummyModel> models = properties[4].Value.Value as IList<DummyModel>;
            Assert.NotNull(models);
            Assert.Equal(complexModel.Models.Count, models.Count);
            Assert.True(complexModel.Models.SequenceEqual(models));
            Action<DummyModel>[] inspectors = new Action<DummyModel>[models.Count];
            for (int i = 0; i < models.Count; i++)
            {
                int index = i;
                inspectors[i] = resource => {
                    Assert.Equal(complexModel.Models[index], resource);
                };
            }
            Assert.Collection(models, inspectors);

            DummyModel[] modelsArray = properties[5].Value.Value as DummyModel[];
            Assert.NotNull(modelsArray);
            Assert.Equal(complexModel.ModelsArray.Length, modelsArray.Length);
            inspectors = new Action<DummyModel>[modelsArray.Length];
            for (int i = 0; i < models.Count; i++)
            {
                int index = i;
                inspectors[i] = resource => {
                    Assert.Equal(complexModel.ModelsArray[index], resource);
                };
            }
            Assert.Collection(modelsArray, inspectors);

            Assert.Equal(complexModel.Resource, properties[6].Value.Value);
        }

        [Fact]
        public void CreateResoureFromComplexModel()
        {
            DummyComplexModel complexModel = new DummyComplexModel() {
                Id = 123,
                Model = new DummyModel(),
                SimpleList = new List<int>() { 1, 2, 3 },
                SimpleArray = new int[] { 1, 2 },
                Models = new List<DummyModel>() { new DummyModel(), new DummyModel() { Floating = 10 } }
            };
            complexModel.ModelsArray = complexModel.Models.ToArray();

            Resource resource = new Resource(typeof(DummyComplexModel));
            resource.UpdateProperties(complexModel);
            KeyValuePair<string, ResourceProperty>[] properties = resource.GetProperties().ToArray();

            Assert.Equal(5, properties.Length);
            Assert.Equal(nameof(DummyComplexModel.Id), properties[0].Key);
            Assert.Equal(nameof(DummyComplexModel.Model), properties[1].Key);
            Assert.Equal(nameof(DummyComplexModel.SimpleList), properties[2].Key);
            Assert.Equal(nameof(DummyComplexModel.SimpleArray), properties[3].Key);
            Assert.Equal(nameof(DummyComplexModel.Models), properties[4].Key);
            Assert.Equal(nameof(DummyComplexModel.ModelsArray), properties[5].Key);
            Assert.Equal(nameof(DummyComplexModel.Resource), properties[6].Key);

            Assert.Equal(typeof(int), properties[0].Value.Type);
            Assert.Equal(typeof(Resource), properties[1].Value.Type);
            Assert.Equal(typeof(IList<int>), properties[2].Value.Type);
            Assert.Equal(typeof(int[]), properties[3].Value.Type);
            Assert.Equal(typeof(List<IResource>), properties[4].Value.Type);
            Assert.Equal(typeof(IResource[]), properties[5].Value.Type);
            Assert.Equal(typeof(CustomResource), properties[6].Value.Type);

            Assert.Equal(typeof(int), properties[0].Value.DeclaredType);
            Assert.Equal(typeof(DummyModel), properties[1].Value.DeclaredType);
            Assert.Equal(typeof(IList<int>), properties[2].Value.DeclaredType);
            Assert.Equal(typeof(int[]), properties[3].Value.DeclaredType);
            Assert.Equal(typeof(IList<DummyModel>), properties[4].Value.DeclaredType);
            Assert.Equal(typeof(DummyModel[]), properties[5].Value.DeclaredType);
            Assert.Equal(typeof(CustomResource), properties[6].Value.DeclaredType);

            Assert.Equal(complexModel.Id, properties[0].Value.Value);
            Assert.Equal(new Resource(typeof(DummyModel)), properties[1].Value.Value);

            IList<int> simpleList = properties[2].Value.Value as IList<int>;
            Assert.NotNull(simpleList);
            Assert.Equal(complexModel.SimpleList.Count, simpleList.Count);
            Assert.True(complexModel.SimpleList.SequenceEqual(simpleList));

            int[] simpleArray = properties[3].Value.Value as int[];
            Assert.NotNull(simpleArray);
            Assert.Equal(complexModel.SimpleArray.Length, simpleArray.Length);
            Assert.True(complexModel.SimpleArray.SequenceEqual(simpleArray));

            List<IResource> models = properties[4].Value.Value as List<IResource>;
            Assert.NotNull(models);
            Assert.Equal(complexModel.Models.Count, models.Count);
            Action<IResource>[] inspectors = new Action<IResource>[models.Count];
            for (int i = 0; i < models.Count; i++)
            {
                int index = i;
                inspectors[i] = resource => {
                    Resource expectedResource = new Resource(typeof(DummyModel));
                    expectedResource.UpdateProperties(complexModel.Models[index]);
                    Assert.Equal(expectedResource, resource);
                };
            }
            Assert.Collection(models, inspectors);

            IResource[] modelsArray = properties[5].Value.Value as IResource[];
            Assert.NotNull(modelsArray);
            Assert.Equal(complexModel.ModelsArray.Length, modelsArray.Length);
            inspectors = new Action<IResource>[modelsArray.Length];
            for (int i = 0; i < models.Count; i++)
            {
                int index = i;
                inspectors[i] = resource => {
                    Resource expectedResource = new Resource(typeof(DummyModel));
                    expectedResource.UpdateProperties(complexModel.ModelsArray[index]);
                    Assert.Equal(expectedResource, resource);
                };
            }
            Assert.Collection(modelsArray, inspectors);

            Assert.Equal(complexModel.Resource, properties[6].Value.Value);
        }

        [Fact]
        public void CreateObservableResoureFromComplexModel()
        {
            DummyComplexModel complexModel = new DummyComplexModel() {
                Id = 123,
                Model = new DummyModel(),
                SimpleList = new List<int>() { 1, 2, 3 },
                SimpleArray = new int[] { 1, 2 },
                Models = new List<DummyModel>() { new DummyModel(), new DummyModel() { Floating = 10 } }
            };
            complexModel.ModelsArray = complexModel.Models.ToArray();

            ObservableResource resource = new ObservableResource(typeof(DummyComplexModel));
            resource.UpdateProperties(complexModel);
            KeyValuePair<string, ResourceProperty>[] properties = resource.GetProperties().ToArray();

            Assert.IsType<ObservableResource>(properties[1].Value.Value);

            Assert.IsType<ObservableCollection<int>>(properties[2].Value.Value);

            Assert.IsType<int[]>(properties[3].Value.Value);

            List<IResource> models = properties[4].Value.Value as List<IResource>;
            Assert.NotNull(models);
            Assert.Equal(complexModel.Models.Count, models.Count);
            Action<IResource>[] inspectors = new Action<IResource>[models.Count];
            for (int i = 0; i < models.Count; i++)
            {
                int index = i;
                inspectors[i] = resource => {
                    Assert.IsType<ObservableResource>(resource);
                };
            }
            Assert.Collection(models, inspectors);

            IResource[] modelsArray = properties[5].Value.Value as IResource[];
            Assert.NotNull(modelsArray);
            Assert.Equal(complexModel.ModelsArray.Length, modelsArray.Length);
            inspectors = new Action<IResource>[modelsArray.Length];
            for (int i = 0; i < models.Count; i++)
            {
                int index = i;
                inspectors[i] = resource => {
                    Assert.IsType<ObservableResource>(resource);
                };
            }
            Assert.Collection(modelsArray, inspectors);

            Assert.Null(properties[6].Value.Value);
        }

        [Fact]
        public void ObservableResourePropertyChanged()
        {
            DummyComplexModel complexModel = new DummyComplexModel() {
                Id = 123,
                Model = new DummyModel(),
                Models = new List<DummyModel>() { new DummyModel(), new DummyModel() { Floating = 10 } }
            };

            ObservableResource resource = new ObservableResource(typeof(DummyComplexModel));
            resource.UpdateProperties(complexModel);

            Assert.PropertyChanged(resource, nameof(DummyComplexModel.Id), () => resource[nameof(DummyComplexModel.Id)] = 2);

            Assert.PropertyChanged(resource.GetValue<ObservableResource>(nameof(DummyComplexModel.Model)), nameof(DummyModel.Floating), () => {
                resource.GetValue<IResource>(nameof(DummyComplexModel.Model))[nameof(DummyModel.Floating)] = 10.10f;
            });

            Assert.PropertyChanged(resource.GetValue<ObservableCollection<IResource>>(nameof(DummyComplexModel.Models)), "Item[]", () => {
                IList<IResource> models = resource.GetValue<IList<IResource>>(nameof(DummyComplexModel.Models));
                models.Add(new Resource(new DummyModel() { Floating = 11 }));
            });
        }
    }
}
