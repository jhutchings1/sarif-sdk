﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Json.Pointer;
using System;

namespace Microsoft.CodeAnalysis.Sarif.Multitool.Rules
{
    public class ReferToFinalSchema : SarifValidationSkimmerBase
    {
        public ReferToFinalSchema() : base(
            RuleId.ReferToFinalSchema,
            RuleResources.SARIF1020_ReferToFinalSchema,
            FailureLevel.Error,
            new string[]
            {
                nameof(RuleResources.SARIF1020_ReferenceToOldSchemaVersion),
                nameof(RuleResources.SARIF1020_SchemaReferenceMissing)
            }
            )
        { }

        protected override void Analyze(SarifLog log, string logPointer)
        {
            AnalyzeSchema(log.SchemaUri, logPointer);
        }

        private void AnalyzeSchema(Uri schemaUri, string pointer)
        {
            if (schemaUri == null)
            {
                LogResult(pointer, nameof(RuleResources.SARIF1020_SchemaReferenceMissing));
                return;
            }

            if (!schemaUri.OriginalString.EndsWith(VersionConstants.StableSarifVersion)
                && !schemaUri.OriginalString.EndsWith($"{VersionConstants.StableSarifVersion}.json")
                && !schemaUri.OriginalString.EndsWith(VersionConstants.SchemaVersionAsPublishedToSchemaStoreOrg)
                && !schemaUri.OriginalString.EndsWith($"{VersionConstants.SchemaVersionAsPublishedToSchemaStoreOrg}.json"))
            {
                LogResult(pointer.AtProperty(SarifPropertyName.Schema), nameof(RuleResources.SARIF1020_ReferenceToOldSchemaVersion), schemaUri.OriginalString);
                return;
            }
        }
    }
}