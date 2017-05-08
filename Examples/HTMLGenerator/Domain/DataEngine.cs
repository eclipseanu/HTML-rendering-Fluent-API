﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.FluentAPI;
using API.Controls;
using API.Infrastructure;

namespace Domain
{
    public class DataEngine : Engine
    {
        public string GenerateForm(int entityDefinitionId)
        {
            //load entity definition from database.
            var entity = new EntityDefinition { Id = entityDefinitionId };
            return Container().Class("container")
                    .Element(
                        Form().Elements(
                            entity.PropertyDefinitions
                                .ToList()
                                .Select(e =>
                                    Container().Class("form-group").Elements(
                                        Label(e.Name).Class("form-label"),
                                        Input(e).Class("form-group")))
                                .ToArray()
                        )
                    ).Html();
        }

        public string GenerateTable(int entityDefinitionId)
        {
            //load all entities for the definition
            var entityList = new List<Entity>();

            return Container().Class("container").Element(
                        Table().Class("table table-bordered").Elements(
                            entityList.Select(e =>
                                DataRow(e))
                            .ToArray()
                            )
                     ).Html();
        }

        private Control Input(PropertyDefinition property)
        {
            var input = new Input();
            input.InputType = GetInputType(property);
            return input;
        }

        private string GetInputType(PropertyDefinition property)
        {
            if (property == null)
                return "text";

            switch (property.Type)
            {
                case PropertyType.Boolean:
                    return "checkbox";
                case PropertyType.Date:
                    return "date";
                case PropertyType.DateTime:
                    return "datetime";
                default:
                    return "text";
            }
        }

        private Control DataRow(Entity entity)
        {
            var tableRow = new TableRow();
            tableRow.Values = entity.Properties.Select(p => p.Value).ToList();
            return tableRow;
        }
    }
}