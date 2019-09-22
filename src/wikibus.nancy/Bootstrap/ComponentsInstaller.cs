﻿using System;
using Argolis.Hydra.Discovery.SupportedOperations;
using Argolis.Models;
using JsonLD.Entities;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Rdf.ModelBinding;
using Nancy.Routing;
using VDS.RDF.Query;
using Wikibus.Common;
using Wikibus.Sources;
using ISourcesDatabaseSettings = Wikibus.Sources.DotNetRDF.ISourcesDatabaseSettings;

namespace Wikibus.Nancy
{
    /// <summary>
    /// Install all required components
    /// </summary>
    public class ComponentsInstaller : Registrations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentsInstaller"/> class.
        /// </summary>
        /// <param name="databaseSettings">Database configuration provider</param>
        /// <param name="catalog">Nancy type catalog</param>
        public ComponentsInstaller(ISourcesDatabaseSettings databaseSettings, ITypeCatalog catalog, IConfiguration c)
            : base(catalog)
        {
            IWikibusConfiguration configuration = new AppSettingsConfiguration(c);

            this.Register(configuration);
            this.Register(new Lazy<ISparqlQueryProcessor>(() =>
            {
                var endpointUri = databaseSettings.SourcesSparqlEndpoint;
                return new RemoteQueryProcessor(new SparqlRemoteEndpoint(endpointUri));
            }));
            this.Register<IFrameProvider>(new WikibusModelFrames());
            this.Register<DefaultRouteResolver>();
            this.Register<IRdfConverter>(typeof(RdfConverter));
            this.RegisterAll<ISupportedOperations>(Lifetime.PerRequest);
        }
    }
}
