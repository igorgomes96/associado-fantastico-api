﻿using AssociadoFantastico.Application.Exceptions;
using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Domain.Exceptions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociadoFantastico.Application.Implementation
{
    public class AppServiceBase<TEntity, TEntityDTO> : IAppServiceBase<TEntity, TEntityDTO> where TEntity : Entity where TEntityDTO : EntityViewModel
    {
        protected readonly IRepositoryBase<TEntity> _repositoryBase;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public AppServiceBase(IUnitOfWork unitOfWork, IRepositoryBase<TEntity> repositoryBase, IMapper mapper, string resourceName, char resourcerGender)
        {
            _repositoryBase = repositoryBase;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            ResourceName = resourceName;
            ResourceGender = resourcerGender;

        }

        public virtual TEntityDTO Adicionar(TEntityDTO obj)
        {
            return _mapper.Map<TEntityDTO>(Adicionar(_mapper.Map<TEntity>(obj)));
        }

        public virtual TEntityDTO Adicionar(TEntity entity)
        {
            var newObj = _repositoryBase.Adicionar(entity);
            _unitOfWork.Commit();
            return _mapper.Map<TEntityDTO>(newObj);
        }


        public virtual IEnumerable<TEntityDTO> BuscarTodos() => 
            _repositoryBase.BuscarTodos().AsQueryable().ProjectTo<TEntityDTO>(_mapper.ConfigurationProvider);

        public virtual TEntityDTO BuscarPeloId(Guid id)
        {
            var entity = _repositoryBase.BuscarPeloId(id);
            if (entity == null) throw new NotFoundException("Código não encontrado.");
            return _mapper.Map<TEntityDTO>(entity);
        }

        public virtual TEntityDTO Excluir(Guid id)
        {
            TEntity obj = _repositoryBase.BuscarPeloId(id);
            if (obj == null) throw new NotFoundException("Código não encontrado.");
            return Excluir(obj);
        }

        public virtual TEntityDTO Excluir(TEntity obj)
        {
            _repositoryBase.Excluir(obj);
            _unitOfWork.Commit();
            return _mapper.Map<TEntityDTO>(obj);
        }

        public virtual void Atualizar(Guid id, TEntityDTO obj)
        {
            _repositoryBase.Atualizar(id, _mapper.Map<TEntity>(obj));
            _unitOfWork.Commit();
        }

        public virtual void Atualizar(TEntity obj)
        {
            _repositoryBase.Atualizar(obj);
            _unitOfWork.Commit();
        }

        public virtual void Dispose()
        {
            _repositoryBase.Dispose();
            _unitOfWork.Dispose();
        }


        // Helpers and Guard Clauses
        protected string ResourceName { get; set; }
        protected char ResourceGender { get; set; }
        protected TEntity BuscarEntidade(Guid id)
        {
            var entity = _repositoryBase.BuscarPeloId(id);
            IsNotNull(entity);
            return entity;
        }

        protected void IsNotNull(object value)
        {
            IsNotNull(value, ResourceName, ResourceGender);
        }

        protected void IsNotNull(object value, string resourceName, char resourceGender)
        {
            if (value == null)
                throw new NotFoundException($"{resourceName} não encontrad{resourceGender}!");
        }
    }
}
