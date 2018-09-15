﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlotAPI.DataStore;
using SlotAPI.DataStores;
using SlotAPI.Domains;
using SlotAPI.Domains.Impl;

namespace SlotAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                
            });

            services.AddTransient<ITransactionHistoryDataStore, TransactionHistoryDataStore>();
            services.AddTransient<IAccountCreditsDataStore, AccountCreditDataStore>();
            services.AddTransient<IAccountDetailsDataStore, AccountDetailsDataStore>();
            services.AddTransient<IStatisticsDataStore, StatisticsDataStore>();

            services.AddTransient<IReel, Reel>();
            services.AddTransient<IGame, Game>();
            services.AddTransient<IAccount, Account>();
            services.AddTransient<ITransaction, Transaction>();
            services.AddTransient<IWin, Win>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
